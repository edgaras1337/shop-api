using api.CustomExceptions;
using api.Data;
using api.Dtos.CommentControllerDtos;
using api.Helpers;
using api.Models;
using AutoMapper;
using System.Security.Claims;

namespace api.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IItemRepository itemRepository,
            IImageService imageService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _imageService = imageService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AddCommentResponse> AddCommentAsync(AddCommentRequest dto)
        {
            // get user, item, and its comments
            var user = await AuthorizeUserAsync();
            var item = await GetItemAsync(dto.ItemId);

            // map the data from dto, to comment model
            var comment = _mapper.Map<Comment>(dto);

            comment.UserId = user.Id;
            comment.CreatedDate = DateTimeOffset.Now;
            comment.ModifiedDate = comment.CreatedDate;

            // add comment to item comments and save changes
            item.Comments.Add(comment);
            await _itemRepository.SaveChangesAsync();

            item.Comments.ForEach(async comment =>
            {
                // append images
                comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName);
                comment.Item!.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            return _mapper.Map<AddCommentResponse>(item);
        }

        public async Task<UpdateCommentResponse> UpdateCommentAsync(UpdateCommentRequest dto)
        {
            var user = await AuthorizeUserAsync();

            var comment = await _commentRepository.GetByIdAsync(dto.Id);
            if (comment is null)
            {
                throw new ObjectNotFoundException();
            }

            bool isValid = false;
            /*user.UserRoles.ForEach(userRole =>
            {
                if (userRole.Role!.RoleName == "Administrator")
                {
                    isValid = true;
                }
            });*/
            if (user.Id == comment.UserId)
            {
                isValid = true;
            }

            if (!isValid)
            {
                throw new UnauthorizedAccessException();
            }

            _mapper.Map(dto, comment);
            await _commentRepository.SaveChangesAsync();

            var item = await GetItemAsync(comment.ItemId);

            item.Comments.ForEach(async comment =>
            {
                // append images
                comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName);
                comment.Item!.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            return _mapper.Map<UpdateCommentResponse>(item);
        }

        public async Task<RemoveCommentResponse> RemoveCommentByIdAsync(int commentId)
        {
            var user = await AuthorizeUserAsync();

            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment is null)
            {
                throw new ObjectNotFoundException();
            }

            bool isValid = false;
            user.UserRoles.ForEach(userRole =>
            {
                if (userRole.Role!.RoleName == "Administrator")
                {
                    isValid = true;
                }
            });
            if (user.Id == comment.UserId)
            {
                isValid = true;
            }

            if (!isValid)
            {
                throw new UnauthorizedAccessException();
            }

            var item = await GetItemAsync(comment.ItemId);

            item.Comments.Remove(comment);
            await _itemRepository.SaveChangesAsync();


            item.Comments.ForEach(async comment =>
            {
                // append images
                comment.User!.ImageSrc = await _imageService.GetImageSourceAsync(comment.User.ImageName);
                comment.Item!.Images.ForEach(async image =>
                    image.ImageSrc = await _imageService.GetImageSourceAsync(image.ImageName));
            });

            return _mapper.Map<RemoveCommentResponse>(item);
        }


        // helpers
        private async Task<User> AuthorizeUserAsync()
        {
            var id = _httpContextAccessor.HttpContext?
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
            return user;
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
