<Newtonsoft.Json.JsonObject()>
<Serializable()>
Public Class Tieup_Codes
    Inherits Base

    Public Property pk_tieup_code_id As String '(varchar(255), not null)
    Public Property tieup_codes As String '(varchar(255), null)
    Public Property file_type As String '(varchar(10), null)
    'Public Property username As String '(varchar(20), null)
    'Public Property dt_last_chg As DateTime '(datetime, null)
    Public Property status As String '(nvarchar(50), null)
    Public Property remarks As String '(varchar(80), null)
    Public Property start_record As String '(varchar(30), null)
    Public Property settlement_currency As String '(varchar(50), null)
    Public Property filename_validation As String
    Public Property footer_validation As String

    Const m_ORDER As String = "dt_last_chg"

    Public Overloads Shared Function order() As String
        Return m_ORDER
    End Function

    Public Overloads Shared Function hasAudit() As Boolean
        Return False
    End Function

End Class
