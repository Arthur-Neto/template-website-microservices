using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule;
using Template.Domain.UsersModule.Enums;
using Template.WebApi.Attributes;

namespace Template.WebApi.Controllers.OData.UsersModule
{
    [ApiController]
    [Route("odata/users")]
    public class UserODataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserODataController(
            IMapper mapper,
            IUserRepository userRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<UserModel>), 200)]
        public IActionResult GetUsers()
        {
            return Ok(_userRepository.RetrieveOData<UserModel>(_mapper));
        }
    }
}
