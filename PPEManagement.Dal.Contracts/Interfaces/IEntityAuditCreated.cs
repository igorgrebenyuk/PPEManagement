namespace PPEManagement.Dal.Contracts.Interfaces
{
    /// <summary>
    /// Аудит создания сущностей
    /// </summary>
    public interface IEntityAuditCreated
    {
        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Кто создал
        /// </summary>
        public string CreatedBy { get; set; }
    }
}