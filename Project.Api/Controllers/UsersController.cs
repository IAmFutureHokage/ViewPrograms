using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Core.Entities;
using Project.Core.Helpers;
using Project.Core.Models.Dto;
using Project.Core.Models.Dto.User;
using Project.Core.Models.Request;
using Project.Core.Models.Response;
using Project.Core.OperationInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v{v:apiVersion}/user")]
    [ApiController, ApiVersion("1")]
 
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly IFileService _fileService;

        public UsersController(IMapper mapper, IUserService userService, ILogger<UsersController> logger, IFileService fileService)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _fileService = fileService;

        }

        [HttpPost("add")]
        public async Task<ResultRequest<DtoAddUser>> Add([FromBody] InboundRequest<DtoAddUser> request)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dto = request?.Data;
                    if (dto == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest<DtoAddUser>.Error("Adding element failed", "Invalid request data");
                    }

                    var user = _mapper.Map<User>(dto);

                    var addedUser = _userService.Add(user, dto.NewPassword);
                    var mappedUser = _mapper.Map<DtoAddUser>(addedUser);
                    await transaction.CommitAsync();
                    return ResultRequest<DtoAddUser>.Ok(mappedUser);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Update element User error", e.ToString());
                    return ResultRequest<DtoAddUser>.Error("Adding element error", e.Message);
                }
            }
        }

        [Authorize]
        [HttpPost("get/me")]
        public ResultRequest<DtoUser> GetMe([FromBody] InboundRequest request)
        {
            try
            {
                var userid = User.GetCurrentUserId();

                var user = _userService.Get<User>(userid);

                var dtoUser = _mapper.Map<DtoUser>(user);

                return ResultRequest<DtoUser>.Ok(dtoUser);
            }
            catch (Exception e)
            {
                _logger.LogError("Get elements User error", e.ToString());
                return ResultRequest<DtoUser>.Error("Error", e.ToString());
            }
        }

        [Authorize]
        [HttpPost("update-my-user")]
        public async Task<ResultRequest> UpdateMyUser([FromBody] InboundRequest<DtoEditMyUser> request)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dto = request?.Data;
                    if (dto == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Invalid request data");
                    }
                    var dbUser = _userService.Get<User>(User.GetCurrentUserId());

                    if (dbUser == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Wrong User");
                    }
                    var avatarFile = dto.AvatarFile;
                    if (avatarFile != null)
                    {
                        try
                        {
                            dbUser.Avatar = await _fileService.SaveFile(avatarFile);
                        }
                        catch (Exception e)
                        {
                            await transaction.RollbackAsync();
                            return ResultRequest.Error("Error", e.Message);
                        }
                    }

                    if (!String.IsNullOrEmpty(dto.NewPassword))
                    {
                        _userService.UpdateUser(dbUser, dto.NewPassword);
                    }
                    else
                    {
                        _userService.UpdateUser(dbUser);
                    }
                    await transaction.CommitAsync();
                    return ResultRequest.Ok();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Update element User error", e.ToString());
                    return ResultRequest.Error("Error", e.ToString());
                }
            }
        }

        [HttpPost("get/{id}")]
        public ResultRequest<DtoUser> Get(Guid id)
        {
            try
            {

                var user = _userService.Get<User>(id);
                var dtoUser = _mapper.Map<DtoUser>(user);
                return ResultRequest<DtoUser>.Ok(dtoUser);
            }
            catch (Exception e)
            {
                _logger.LogError("Get elements User error", e.ToString());
                return ResultRequest<DtoUser>.Error("Error", e.ToString());
            }
        }

        [Authorize]
        [HttpGet("get-page")]
        public async Task<ResultRequest<List<DtoUser>>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                return ResultRequest<List<DtoUser>>.Ok(_mapper.Map<List<DtoUser>>(await _userService.GetPageAsync<User>(pageNumber, pageSize)));
            }
            catch (Exception e)
            {
                _logger.LogError("Get all elements User error", e.ToString());
                return ResultRequest<List<DtoUser>>.Error("Get all elements user error", e.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("delete/{id}")]
        public async Task<ResultRequest> Delete(Guid id)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dbUser = await _userService.GetAsync<User>(id);
                    _userService.Delete(dbUser);
                    await transaction.CommitAsync();
                    return ResultRequest.Ok();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Deleting element User error", e.ToString());
                    return ResultRequest.Error("Deleting element User error", e.Message);
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("update")]
        public async Task<ResultRequest> Update([FromBody] InboundRequest<DtoEditUser> request)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dto = request?.Data;
                    if (dto == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Invalid request data");
                    }
                    var dbUser = _userService.Get<User>(dto.Id);

                    if (dbUser == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Wrong User");
                    }
                    _mapper.Map(dto, dbUser);
                    if (!String.IsNullOrEmpty(dto.NewPassword))
                    {
                        _userService.UpdateUser(dbUser, dto.NewPassword);
                    }
                    else
                    {
                        _userService.UpdateUser(dbUser);
                    }
                    await transaction.CommitAsync();
                    return ResultRequest.Ok();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Update element User error", e.ToString());
                    return ResultRequest.Error("Error", e.ToString());
                }
            }
        }
       
    }
}
