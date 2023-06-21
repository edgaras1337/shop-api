using api.Helpers;
using AutoMapper;

namespace api.Services
{
    public class CommonService
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CommonService(IMapper mapper, IImageService imageService)
        {
            _mapper = mapper;
            _imageService = imageService;
        }

        protected IMapper Mapper => _mapper;

        protected TTo Map<TTo>(object obj)
        {
            return _mapper.Map<TTo>(obj);
        }

        //protected TEntity GetDataWithImage<TEntity>(object obj)
        //{

        //}

        protected virtual Task OnGetData<TEntity>(TEntity entity)
        {
            return Task.FromResult(entity);
            //await _imageService.LoadImagesAsync(entity);
        }
    }
}
