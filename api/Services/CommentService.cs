using api.CustomExceptions;
using api.Data;
using api.Dtos.AuthControllerDtos;
using api.Dtos.CommentControllerDtos;
using api.Helpers;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IUserService _userService;
        private readonly IItemRepository _itemRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(
            ICommentRepository commentRepository,
            UserManager<ApplicationUser> userManager,
            //IUserService userService,
            IItemRepository itemRepository,
            IImageService imageService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _userManager = userManager;
            //_userService = userService;
            _itemRepository = itemRepository;
            _imageService = imageService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AddCommentResponse> AddCommentAsync(AddCommentRequest dto)
        {
            // get user, item, and its comments
            //var user = await AuthorizeUserAsync();

            var user = await AuthorizeUserAsync();
            var item = await GetItemAsync(dto.ItemId);

            // map the data from dto, to comment model
            var comment = _mapper.Map<Comment>(dto);

            comment.UserId = user.Id;
            comment.CreatedDate = DateTimeOffset.UtcNow;
            comment.ModifiedDate = comment.CreatedDate;

            // add comment to item comments and save changes
            item.Comments.Add(comment);
            await _itemRepository.SaveChangesAsync();

            item = await item.WithImagesAsync(_imageService);

            var resDto = _mapper.Map<AddCommentResponse>(item);

            return resDto;

            /*item.Comments.ForEach(async comment =>
            {
                // append images
                comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName);
                comment.Item!.Images.ForEach(async image =>
                    image.ImageSource = await _imageService.GetImageSourceAsync(image.ImageName));
            });*/


        }

        public async Task<UpdateCommentResponse> UpdateCommentAsync(UpdateCommentRequest dto)
        {
            var user = await AuthorizeUserAsync();
            var comment = await _commentRepository.GetByIdAsync(dto.Id);

            if (comment is null)
            {
                throw new ObjectNotFoundException();
            }

            if (user.Id == comment.UserId || await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                _mapper.Map(dto, comment);

                comment.ModifiedDate = DateTimeOffset.UtcNow;

                await _commentRepository.SaveChangesAsync();

                var item = await GetItemAsync(comment.ItemId);

                item = await item.WithImagesAsync(_imageService);

                var resDto = _mapper.Map<UpdateCommentResponse>(item);

                return resDto;
            }

            throw new UnauthorizedException();
        }

        public async Task<RemoveCommentResponse> RemoveCommentByIdAsync(int commentId)
        {
            var user = await AuthorizeUserAsync();
            var comment = await _commentRepository.GetByIdAsync(commentId);

            if (comment is null)
            {
                throw new ObjectNotFoundException();
            }

            if (user.Id == comment.UserId || await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                var item = await GetItemAsync(comment.ItemId);

                item.Comments.Remove(comment);
                await _itemRepository.SaveChangesAsync();


                item = await item.WithImagesAsync(_imageService);

                var resDto = _mapper.Map<RemoveCommentResponse>(item);

                return resDto;
            }

            throw new UnauthorizedException();
        }


        // helpers
        private async Task<ApplicationUser> AuthorizeUserAsync()
        {
            var name = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (name is null)
            {
                throw new UnauthorizedException();
            }

            var user = await _userManager.Users
                .SingleOrDefaultAsync(e => e.UserName == name);

            if (user is null)
            {
                throw new UnauthorizedException();
            }

            return user;

            /*var id = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                throw new UnauthorizedAccessException();
            }
            var user = await _userRepository.GetByIdAsync(int.Parse(id));
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }
            return user;*/
        }

        private async Task<Item> GetItemAsync(int itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item is null)
            {
                throw new ObjectNotFoundException();
            }
            return item;
        }
    }
}
