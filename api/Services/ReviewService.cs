using api.CustomExceptions;
using api.Dtos.CommentControllerDtos;
using api.Helpers;
using api.Models;
using api.Repo;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class ReviewService : IReviewService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(
            UserManager<ApplicationUser> userManager,
            IImageService imageService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _imageService = imageService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddReviewResponse> AddReviewAsync(AddReviewRequest dto)
        {
            var user = await AuthorizeUserAsync();

            var review = _mapper.Map<ItemReview>(dto);
            review.UserId = user.Id;
            review.CreatedDate = DateTimeOffset.UtcNow;
            review.ModifiedDate = review.CreatedDate;

            await _unitOfWork.ItemReviewRepository.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            await _imageService.LoadImagesAsync(review.User!);
            return _mapper.Map<AddReviewResponse>(review);
        }

        public async Task<UpdateReviewResponse> UpdateReviewAsync(UpdateReviewRequest dto)
        {
            var user = await AuthorizeUserAsync();
            var review = await _unitOfWork.ItemReviewRepository
                .GetAllQuery()
                .Include(e => e.User)
                .ThenInclude(e => e!.UserRoles)
                .ThenInclude(e => e.Role)
                .SingleOrDefaultAsync(e => e.Id == dto.Id);

            if (review == null)
            {
                throw new ObjectNotFoundException();
            }

            if (user.Id == review.UserId || await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                _mapper.Map(dto, review);
                review.ModifiedDate = DateTimeOffset.UtcNow;
                await _unitOfWork.SaveChangesAsync();

                await _imageService.LoadImagesAsync(review.User!);
                return _mapper.Map<UpdateReviewResponse>(review);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task RemoveCommentByIdAsync(int commentId)
        {
            var user = await AuthorizeUserAsync();
            var review = await _unitOfWork.ItemReviewRepository.GetByIdAsync(commentId);
            if (review is null)
            {
                throw new ObjectNotFoundException();
            }

            if (user.Id == review.UserId || await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                _unitOfWork.ItemReviewRepository.Delete(review);
                await _unitOfWork.SaveChangesAsync();
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
        }
    }
}
