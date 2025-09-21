using System;

namespace FinalExamProject.Models
{
    public class AdminDashboardVm
    {
        public int ProductCount { get; set; }
        public int CategoryCount { get; set; }
        public int OrderCount { get; set; }
        public int MemberCount { get; set; }

        public int InventoryLowCount { get; set; }      // 低於安全存量的品項數
        public int PendingShipment { get; set; }        // 出貨中的筆數
        public int PaymentPending { get; set; }         // 等待付款的筆數
    }
}
