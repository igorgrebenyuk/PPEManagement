namespace PPEManagement.Dal.Contracts.Interfaces
{
    /// <summary>
    /// Аудит удаления сущностей
    /// </summary>
    public interface IEntityAuditDeletedAt
    {
        /// <summary>
        /// Дата и время мягкого удаления
        /// </summary>
        public DateTimeOffset? DeletedAt { get; set; }
    }
}