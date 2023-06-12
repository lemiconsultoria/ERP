using AutoMapper;
using ERP.Core.Commands;
using ERP.Core.DTOs;
using ERP.Core.Entities;
using ERP.Core.Messages;

namespace ERP.Core.AutoMapper
{
    public static class Mapper
    {
        public static TEntity CommandToEntity<TCommand, TEntity>(TCommand command) where TEntity : Entity where TCommand : CommandBase
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TCommand, TEntity>());

            var mapper = config.CreateMapper();
            return mapper.Map<TEntity>(command);
        }

        public static TCommand EntityToCommand<TEntity, TCommand>(TEntity command) where TEntity : Entity where TCommand : CommandBase
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TCommand>());

            var mapper = config.CreateMapper();
            return mapper.Map<TCommand>(command);
        }

        public static TData EntityToCommandResult<TEntity, TData>(TEntity command) where TEntity : Entity where TData : CommandDataResult
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TData>());

            var mapper = config.CreateMapper();
            return mapper.Map<TData>(command);
        }

        public static TEvent EntityToEvent<TEntity, TEvent>(TEntity command) where TEntity : Entity where TEvent : Message
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TEvent>());

            var mapper = config.CreateMapper();
            return mapper.Map<TEvent>(command);
        }

        public static TDto EntityToDTO<TEntity, TDto>(TEntity command) where TEntity : Entity where TDto : DTOBase
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TEntity, TDto>());

            var mapper = config.CreateMapper();
            return mapper.Map<TDto>(command);
        }

        public static TEntity DTOToEntity<TDto, TEntity>(TEntity command) where TDto : DTOBase where TEntity : Entity
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TDto, TEntity>());

            var mapper = config.CreateMapper();
            return mapper.Map<TEntity>(command);
        }

        public static TDto ClassToDTO<TClass, TDto>(TClass command) where TClass : class where TDto : DTOBase
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TClass, TDto>());

            var mapper = config.CreateMapper();
            return mapper.Map<TDto>(command);
        }

        public static TEntity EventToEntity<TEvent, TEntity>(TEvent command) where TEntity : Entity where TEvent : Message
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TEvent, TEntity>());

            var mapper = config.CreateMapper();
            return mapper.Map<TEntity>(command);
        }
    }
}