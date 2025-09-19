namespace XeniaCatalogueApi.Dictionary
{
    public sealed class OrderStatus
    {
        public static string PENDING = "PENDING";
        public static string ORDERED = "ORDERED";
        public static string ORDER_NOT_PLACED = "ORDER_NOT_PLACED";
        public static string ACCEPTED = "ACCEPTED";
        public static string PREPARING = "PREPARING";
        public static string PACKED = "PACKED";
        public static string SHIPPED = "SHIPPED";
        public static string OUT_FOR_DELIVERY = "OUT_FOR_DELIVERY";
        public static string DELIVERED = "DELIVERED";
        public static string CANCEL = "CANCEL";
        public static string CANCELLED = "CANCELLED";
        public static string RETURN = "RETURN";
        public static string RETURNED = "RETURNED";
        public static string REFUND_INITIATED = "REFUND_INITIATED";
        public static string REFUNDED = "REFUNDED";
        private OrderStatus() { }
    }
}
