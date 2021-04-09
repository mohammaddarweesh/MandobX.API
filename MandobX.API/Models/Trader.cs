using MandobX.API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MandobX.API.Authentication
{
    /// <summary>
    /// trader class
    /// </summary>
    public class Trader
    {
        /// <summary>
        /// Trader Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        /// <summary>
        /// Trader Points
        /// </summary>
        public int Points { get; set; }
        /// <summary>
        /// trader's shipment operation
        /// </summary>
        public virtual List<ShipmentOperation> ShipmentOperations { get; set; }
        /// <summary>
        /// type of trading
        /// </summary>
        public TypeOfTrading TypeOftrading{ get; set; }

        /// <summary>
        /// type of trading
        /// </summary>
        public string TypeOftradingId{ get; set; }
        /// <summary>
        /// user id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// user
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Driver first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Driver last name
        /// </summary>
        public string LastName { get; set; }

    }
}
