using PPEManagement.Dal.Contracts.Interfaces;

namespace PPEManagement.Dal.Contracts.Repositories;



    /// <summary>
    /// Базовый интерфейс репозитория записи сущностей
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public interface IBaseWriteRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Добавляет новую сущность
        /// </summary>
        /// <param name="entity">Добавляемая сущность</param>
        void Add(T entity);

        /// <summary>
        /// Обновляет сущность
        /// </summary>
        /// <param name="entity">Обновляемая сущность</param>
        void Update(T entity);

        /// <summary>
        /// Удаляет сущность
        /// </summary>
        /// <param name="entity">Удаляемая сущность</param>
        void Delete(T entity);
    }
