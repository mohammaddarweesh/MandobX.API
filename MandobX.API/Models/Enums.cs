namespace MandobX.API.Models
{
    public enum ShipmentStatus
    {
        Pending = 0,
        AdminAccepted = 1,
        PendingDriver = 2,
        DriverAccepted = 3,
        OnTheWay = 4,
        Shipped = 5,
        AdminRejected = 6,
        DriverRejected = 7,
        TraderAccepted = 8,
        DriverRequested = 9,
    }
    public enum FileType
    {
        Identity = 0,
        Vehicle = 1,
        DrivingLicence = 2,
        TraderDoc = 3,
    }

    public enum ContentType
    {
        AcceptShipment = 0,
        RejectShipment = 1,
        AcceptDriver = 2,
        RejectDriver = 3,
        DriverAcceptShipment = 4,
        DriverRejectShipment = 5,
    }
    public enum Result
    {
        Success = 0,
        Error = 1,
    }
    public enum UserStatus
    {
        Active = 0,
        InActive = 1,
        Blocked = 2,
        PendingMessageApproval = 3,
    }
}
