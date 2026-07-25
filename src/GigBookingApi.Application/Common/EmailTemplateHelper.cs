namespace GigBookingApi.Application.Common;

public class EmailTemplateHelper
{
    public static (string Subject, string HtmlBody) CreateCustomerBookingTemplate(
            string customerName,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string venue,
            string bookingId)
    {
        var subject = "Tack för din bokningsförfrågan!";
        var body = $@"
            <h2>Bokningsförfrågan mottagen</h2>
            <p>Hej {customerName},</p>
            <p>Tack för din förfrågan. Vi har tagit emot dina uppgifter och återkommer inom 48 timmar med bekräftelse.</p>
            <hr />
            <p><strong>Boknings-ID:</strong> {bookingId}</p>
            <p><strong>Datum & Tid:</strong> {startDate:yyyy-MM-dd HH:mm} - {endDate:HH:mm}</p>
            <p><strong>Spelplats:</strong> {venue}</p>";

        return (subject, body);
    }

    public static (string Subject, string HtmlBody) CreateAdminNotificationTemplate(
        string customerName,
        string customerEmail,
        string customerPhone,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string venue,
        string address,
        string bookingId)
    {
        var subject = $"NY BOKNINGSFÖRFRÅGAN: {customerName} - {venue}";
        var body = $@"
            <h2>Ny bokningsförfrågan!</h2>
            <p><strong>Kund:</strong> {customerName}</p>
            <p><strong>E-post:</strong> {customerEmail}</p>
            <p><strong>Telefon:</strong> {customerPhone}</p>
            <p><strong>Datum & Tid:</strong> {startDate:yyyy-MM-dd HH:mm} - {endDate:HH:mm}</p>
            <p><strong>Spelplats:</strong> {venue}</p>
            <p><strong>Adress:</strong> {address}</p>
            <p><strong>Boknings-ID:</strong> {bookingId}</p>";

        return (subject, body);
    }

    public static (string Subject, string HtmlBody) UpdateBookingTemplate(
        string customerName,
        DateTime newDate,
        int newQuantity,
        string bookingId)
    {
        var subject = "Din bokning har uppdaterats";

        var body = $@"
            <h2>Uppdaterad bokning</h2>
            <p>Hej {customerName},</p>
            <p>Din bokning har uppdaterats.</p>
            <p><strong>Boknings-ID:</strong> {bookingId}</p>
            <p><strong>Nytt datum:</strong> {newDate:yyyy-MM-dd}</p>
            <p><strong>Nytt antal:</strong> {newQuantity}</p>
            <p>Kontakta oss om du har frågor.</p>
        ";

        return (subject, body);
    }

    public static (string Subject, string HtmlBody) DeleteBookingTemplate(
        string customerName,
        string bookingId)
    {
        var subject = "Din bokning har avbokats";

        var body = $@"
            <h2>Avbokning</h2>
            <p>Hej {customerName},</p>
            <p>Din bokning har avbokats.</p>
            <p><strong>Boknings-ID:</strong> {bookingId}</p>
            <p>Hoppas vi får se dig en annan gång!</p>
        ";

        return (subject, body);
    }

    public static (string Subject, string HtmlBody) UpdateAdminNotificationTemplate(
        string customerName,
        string customerEmail,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string venue,
        string bookingId)
    {
        var subject = $"UPPDATERAD BOKNING: {customerName} - {venue}";
        var body = $@"
            <h2>En bokning har uppdaterats!</h2>
            <p><strong>Kund:</strong> {customerName} ({customerEmail})</p>
            <p><strong>Boknings-ID:</strong> {bookingId}</p>
            <p><strong>Nya tider:</strong> {startDate:yyyy-MM-dd HH:mm} - {endDate:HH:mm}</p>
            <p><strong>Spelplats:</strong> {venue}</p>";

        return (subject, body);
    }

    // Admin-notis vid AVBOKNING / RADERING
    public static (string Subject, string HtmlBody) DeleteAdminNotificationTemplate(
        string customerName,
        string venue,
        string bookingId)
    {
        var subject = $"AVBOKAT: {customerName} - {venue}";
        var body = $@"
            <h2>En bokning har avbokats/raderats!</h2>
            <p><strong>Kund:</strong> {customerName}</p>
            <p><strong>Spelplats:</strong> {venue}</p>
            <p><strong>Boknings-ID:</strong> {bookingId}</p>
            <p>Tiden i kalendern har nu frigjorts.</p>";

        return (subject, body);
    }


}