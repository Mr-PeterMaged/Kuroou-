
using AutoMapper;
using Ecommerce.Api.Dtos;
using Ecommerce.Api.Errors;
using Ecommerce.Api.Extentions;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
	public class AccountController : BaseController
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;
		public AccountController(UserManager<AppUser> userManager,
								 SignInManager<AppUser> signInManager,
								 ITokenService tokenService,
								 IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}
		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			if (user == null) return Unauthorized(new ApiResponse(401));
			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
			if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

			return new UserDto
			{
				Email = user.Email,
				DisplayName = user.DisplayName,
				Token = _tokenService.CreateToken(user)
			};
		}
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if(CheckEmailExistsAsync(registerDto.Email).Result.Value)
			{
				return new BadRequestObjectResult(new ApiValidationErrorResponse
				{
					Errors =
					[
						"Email Address in Use"
					]
				});
			}

			var user = new AppUser
			{
				DisplayName = registerDto.DisplayName,
				Email = registerDto.Email,
				UserName = registerDto.Email,
			};
			var result = await _userManager.CreateAsync(user);
			if(!result.Succeeded) return BadRequest(new ApiResponse(400));

			return new UserDto
			{
				Email = user.Email,
				DisplayName = user.DisplayName,
				Token = _tokenService.CreateToken(user)
			};
		}

		[HttpGet("check-email")]
		public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
		{
			return await _userManager.FindByEmailAsync(email) != null;
		}

		[HttpGet("current-user")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
			return new UserDto
			{
				Email = user?.Email!,
				DisplayName = user?.DisplayName!,
				Token = _tokenService.CreateToken(user!)
			};
		}
		[HttpGet("address")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<ActionResult<AddressDto>> GetUserAddress()
		{
			var user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);
			var addressDto = _mapper.Map<Address,AddressDto>(user.Address);
			return addressDto;
		}
		[HttpPut("address")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
		{
			var user = await _userManager.FindByEmailWithAddressAsync(HttpContext.User);
			user.Address = _mapper.Map<AddressDto, Address>(address);

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));
			return BadRequest("Problem happend");
		}
	}
}
