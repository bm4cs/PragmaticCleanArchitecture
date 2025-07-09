using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PricingService _pricingService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReserveBookingCommandHandler(
        IUserRepository userRepository,
        IBookingRepository bookingRepository,
        IApartmentRepository apartmentRepository,
        IUnitOfWork unitOfWork,
        PricingService pricingService,
        IDateTimeProvider dateTimeProvider
    )
    {
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
        _apartmentRepository = apartmentRepository;
        _unitOfWork = unitOfWork;
        _pricingService = pricingService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(
        ReserveBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository
            .GetByIdAsync(request.UserId, cancellationToken)
            .ConfigureAwait(false);
        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var apartment = await _apartmentRepository
            .GetByIdAsync(request.ApartmentId, cancellationToken)
            .ConfigureAwait(false);

        if (apartment is null)
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (
            await _bookingRepository
                .IsOverlappingAsync(apartment, duration, cancellationToken)
                .ConfigureAwait(false)
        )
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }

        var booking = Booking.Reserve(
            apartment,
            user.Id,
            duration,
            _dateTimeProvider.UtcNow,
            _pricingService
        );

        _bookingRepository.Add(booking);

        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return booking.Id;
    }
}
