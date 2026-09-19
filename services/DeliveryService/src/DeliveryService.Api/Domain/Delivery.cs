using DeliveryService.Api.Domain.Common;

namespace DeliveryService.Api.Domain
{
    public class Delivery : BaseEntity
    {
        public Address Destination { get; private set; } = null!;

        public decimal Weight { get; private set; }
        public decimal Volume { get; private set; }

        public DeliveryStatus Status { get; private set; }

        private Delivery() { }

        private Delivery(Address destination, decimal weight, decimal volume, DateTime createdAt)
        {
            Destination = destination;
            Weight = weight;
            Volume = volume;
            Status = DeliveryStatus.Pending;
            CreatedAt = createdAt;
            UpdatedAt = createdAt;
        }

        public static Delivery Create(Address destination, decimal weight, decimal volume, DateTime createdAt)
        {
            if (weight <= 0) throw new ArgumentOutOfRangeException(nameof(weight));
            if (volume <= 0) throw new ArgumentOutOfRangeException(nameof(volume));

            return new Delivery(destination, weight, volume, createdAt);
        }
    }

    public enum DeliveryStatus
    {
        Pending,
        Processing,
        InTransit,
        Delivered,
        Cancelled
    }
}
