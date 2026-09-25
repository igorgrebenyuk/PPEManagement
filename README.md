В рамках данного ПР я сделаю Реестр СИЗ , а именно автоматизирую заполнение для ведомомости СИЗ , которую закреплю ниже в РИДМИ файле
<img width="2200" height="1550" alt="Ведомость_выдачи_СИЗ_предпросмотр" src="https://github.com/user-attachments/assets/1d653027-4808-4fa0-bf49-9d8c53c1213d" />
Взаимодействие сущностей в модуле построено на разделении оперативных данных (кадры, склад) и зафиксированных документов (ведомости выдачи).

```mermaid
classDiagram
    class BaseAuditEntity {
        <<Abstract>>
        +Guid Id
        +DateTimeOffset CreatedAt
        +string CreatedBy
        +DateTimeOffset UpdatedAt
        +string UpdatedBy
        +DateTimeOffset? DeletedAt
    }

    class Employee {
        +string PersonnelNumber
        +string FullName
        +string Department
        +string Position
        +int GasMaskSize
        +int SuitSize
        +bool IsNFGO
    }

    class PPECard {
        +string Name
        +string BatchNumber
        +int Size
        +int Quantity
        +DateTime ExpirationDate
        +DateTime NextCheckDate
        +string Status
        +string IssuedToEmployeeFullName
    }

    class PPEStatement {
        +string StatementNumber
        +string OrganizationName
        +string DepartmentName
        +string Reason
        +DateTime IssueDate
        +string ResponsiblePerson
        +int TotalGasMasks
        +int TotalKIMGZ
        +int TotalOtherPPE
        +string SignedScanPath
    }

    class PPEStatementItem {
        +Guid StatementId
        +string EmployeeFullName
        +string PersonnelNumber
        +string PPEName
        +string BatchNumber
        +string Size
        +int Quantity
        +DateTime IssueDate
        +string SignatureStatus
    }

    BaseAuditEntity <|-- Employee : Наследует
    BaseAuditEntity <|-- PPECard : Наследует
    BaseAuditEntity <|-- PPEStatement : Наследует
    BaseAuditEntity <|-- PPEStatementItem : Наследует

    PPEStatement "1" *-- "many" PPEStatementItem : Содержит строки
    Employee ..> PPEStatementItem : Снапшот данных
    PPECard ..> PPEStatementItem : Списание партии
    Employee ..> PPECard : Подбор по размерам
