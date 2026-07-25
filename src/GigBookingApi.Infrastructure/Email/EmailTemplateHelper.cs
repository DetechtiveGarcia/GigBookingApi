namespace GigBookingApi.Infrastructure.Email;
public class EmailTemplateHelper
{
    public static (string Subject, string HtmlBody) CreateBookingTemplate(
        string customerName,
        DateTime bookingDate,
        int quantity,
        string bookingId)
    {
        var subject = "Din bokning är bekräftad";

        var body = $@"
            <h2>Bokningsbekräftelse</h2>
            <p>Hej {customerName},</p>
            <p>Tack för din bokning hos Temolldur.</p>
            <p><strong>Boknings-ID:</strong> {bookingId}</p>
            <p><strong>Datum:</strong> {bookingDate:yyyy-MM-dd}</p>
            <p><strong>Antal:</strong> {quantity}</p>
            <p>Vi ser fram emot att träffa dig!</p>
        ";

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
}
