﻿using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories.Interfaces;
using Dtos;
using Mapster;
using Services.Interfances;

namespace Services
{
    /// <summary>
    /// Owner的增删改查
    /// 构造函数需要IRepositoryManager,这样就与Domain层关联了起来
    /// </summary>
    internal sealed class OwnerService : IOwnerService
    {
        private readonly IRepositoryManager _repositoryManager;

        public OwnerService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<IEnumerable<OwnerDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var owners = await _repositoryManager.OwnerRepository.GetAllAsync(cancellationToken);

            var ownersDto = owners.Adapt<IEnumerable<OwnerDto>>();

            return ownersDto;
        }

        public async Task<OwnerDto> GetByIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

            if (owner is null)
            {
                throw new OwnerNotFoundException(ownerId);
            }

            var ownerDto = owner.Adapt<OwnerDto>();

            return ownerDto;
        }

        public async Task<OwnerDto> CreateAsync(OwnerForCreationDto ownerForCreationDto, CancellationToken cancellationToken = default)
        {
            var owner = ownerForCreationDto.Adapt<Owner>();

            _repositoryManager.OwnerRepository.Insert(owner);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return owner.Adapt<OwnerDto>();
        }

        public async Task UpdateAsync(Guid ownerId, OwnerForUpdateDto ownerForUpdateDto, CancellationToken cancellationToken = default)
        {
            var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

            if (owner is null)
            {
                throw new OwnerNotFoundException(ownerId);
            }

            owner.Name = ownerForUpdateDto.Name;
            owner.DateOfBirth = ownerForUpdateDto.DateOfBirth;
            owner.Address = ownerForUpdateDto.Address;

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            var owner = await _repositoryManager.OwnerRepository.GetByIdAsync(ownerId, cancellationToken);

            if (owner is null)
            {
                throw new OwnerNotFoundException(ownerId);
            }

            _repositoryManager.OwnerRepository.Remove(owner);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}