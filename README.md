В рамках данного ПР я сделаю Реестр СИЗ , а именно автоматизирую заполнение для ведомомости СИЗ , которую закреплю ниже в РИДМИ файле
<img width="2200" height="1550" alt="Ведомость_выдачи_СИЗ_предпросмотр" src="https://github.com/user-attachments/assets/1d653027-4808-4fa0-bf49-9d8c53c1213d" />
Взаимодействие сущностей в модуле построено на разделении оперативных данных (кадры, склад) и зафиксированных документов (ведомости выдачи).

```mermaid
erDiagram
    BaseAuditEntity ||--|| Employee : "наследует аудит"
    BaseAuditEntity ||--|| PPECard : "наследует аудит"
    BaseAuditEntity ||--|| PPEStatement : "наследует аудит"
    BaseAuditEntity ||--|| PPEStatementItem : "наследует аудит"

    PPEStatement ||--|{ PPEStatementItem : "1 : N (содержит строки)"
    
    Employee ..> PPEStatementItem : "копирует данные (PersonnelNumber, FullName)"
    PPECard ..> PPEStatementItem : "привязывает партию и списывает остаток"
    Employee ..> PPECard : "сопоставляется по размерам (GasMaskSize, SuitSize)"

    Employee {
        Guid Id PK
        string PersonnelNumber
        string FullName
        string Department
        int GasMaskSize
        int SuitSize
        bool IsNFGO
    }

    PPECard {
        Guid Id PK
        string Name
        string BatchNumber
        int Size
        int Quantity
        DateTime ExpirationDate
        string Status
    }

    PPEStatement {
        Guid Id PK
        string StatementNumber
        string OrganizationName
        string DepartmentName
        string Reason
        DateTime IssueDate
        int TotalGasMasks
        int TotalKIMGZ
    }

    PPEStatementItem {
        Guid Id PK
        Guid StatementId FK
        string EmployeeFullName
        string PPEName
        string BatchNumber
        string Size
        int Quantity
        string SignatureStatus
    }
