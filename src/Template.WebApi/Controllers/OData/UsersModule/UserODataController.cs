using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
        [ProducesResponseType(typeof(IQueryable<UserModel>), 200)]
        public IActionResult Get()
        {
            return Ok(_userRepository.RetrieveOData<UserModel>(_mapper));
        }
    }
}
