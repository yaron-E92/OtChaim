namespace OtChaim.Domain.EmergencyEvents;

/// <summary>
/// Value object representing attachments and preferences associated with an emergency.
/// This class encapsulates all the additional information and files that can be
/// attached to an emergency notification, as well as contact method preferences.
/// </summary>
/// <remarks>
/// EmergencyAttachments serves as a value object that groups related emergency
/// attachment information together. It includes both file attachments (pictures,
/// documents) and information attachments (personal info, medical info, GPS location),
/// as well as contact method preferences for how the emergency should be communicated.
/// 
/// This class is designed to be immutable and provides a clean interface for
/// managing emergency-related attachments and preferences. It's used throughout
/// the emergency management system to ensure consistent handling of attachment data.
/// </remarks>
public class EmergencyAttachments
{
    /// <summary>
    /// A value indicating whether personal information should be included.
    /// </summary>
    /// <remarks>
    /// When true, the user's personal information (name, contact details, etc.)
    /// will be included in the emergency notification to help emergency responders
    /// identify and contact the person in need.
    /// </remarks>
    public bool IncludePersonalInfo { get; set; }

    /// <summary>
    /// A value indicating whether medical information should be included.
    /// </summary>
    /// <remarks>
    /// When true, the user's medical information (allergies, medications, conditions)
    /// will be included in the emergency notification to help emergency responders
    /// provide appropriate medical care.
    /// </remarks>
    public bool IncludeMedicalInfo { get; set; }

    /// <summary>
    /// A value indicating whether GPS location should be included.
    /// </summary>
    /// <remarks>
    /// When true, the current GPS coordinates will be included in the emergency
    /// notification to help emergency responders locate the incident quickly.
    /// </remarks>
    public bool IncludeGpsLocation { get; set; }

    /// <summary>
    /// Gets or sets the file path to an attached picture.
    /// </summary>
    /// <remarks>
    /// This property stores the path to a picture file that has been selected
    /// by the user to provide visual context for the emergency situation.
    /// The picture can help emergency responders understand the situation better.
    /// </remarks>
    public string PicturePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file path to an attached document.
    /// </summary>
    /// <remarks>
    /// This property stores the path to a document file that has been selected
    /// by the user to provide additional information about the emergency.
    /// Documents might include medical records, insurance information, or other
    /// relevant paperwork.
    /// </remarks>
    public string DocumentPath { get; set; } = string.Empty;

    /// <summary>
    /// A value indicating whether the emergency should be sent via email.
    /// </summary>
    /// <remarks>
    /// When true, emergency notifications will be sent to contacts via email.
    /// This preference allows users to choose their preferred communication method
    /// for emergency notifications.
    /// </remarks>
    public bool SendEmail { get; set; }

    /// <summary>
    /// A value indicating whether the emergency should be sent via SMS.
    /// </summary>
    /// <remarks>
    /// When true, emergency notifications will be sent to contacts via SMS.
    /// SMS provides immediate delivery and is often the fastest way to reach
    /// emergency contacts.
    /// </remarks>
    public bool SendSms { get; set; }

    /// <summary>
    /// A value indicating whether the emergency should be sent via messenger.
    /// </summary>
    /// <remarks>
    /// When true, emergency notifications will be sent to contacts via messenger
    /// applications. This provides an alternative communication channel that
    /// some users may prefer.
    /// </remarks>
    public bool SendMessenger { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyAttachments"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor creates a new EmergencyAttachments instance with default values.
    /// The default configuration includes personal info, medical info, and GPS location
    /// enabled, with email and SMS as the default contact methods.
    /// </remarks>
    public EmergencyAttachments()
    {
        // Default configuration for emergency attachments
        IncludePersonalInfo = true;
        IncludeMedicalInfo = true;
        IncludeGpsLocation = true;
        SendEmail = true;
        SendSms = true;
        SendMessenger = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyAttachments"/> class with specified values.
    /// </summary>
    /// <param name="includePersonalInfo">Whether to include personal information.</param>
    /// <param name="includeMedicalInfo">Whether to include medical information.</param>
    /// <param name="includeGpsLocation">Whether to include GPS location.</param>
    /// <param name="picturePath">Path to an attached picture file.</param>
    /// <param name="documentPath">Path to an attached document file.</param>
    /// <param name="sendEmail">Whether to send notifications via email.</param>
    /// <param name="sendSms">Whether to send notifications via SMS.</param>
    /// <param name="sendMessenger">Whether to send notifications via messenger.</param>
    /// <remarks>
    /// This constructor allows creating an EmergencyAttachments instance with
    /// specific configuration values. It provides full control over which
    /// attachments and contact methods are enabled for the emergency.
    /// </remarks>
    public EmergencyAttachments(
        WhichContactMethods contactMethods,
        bool includePersonalInfo = true,
        bool includeMedicalInfo = true,
        bool includeGpsLocation = true,
        string picturePath = "",
        string documentPath = "")
    {
        IncludePersonalInfo = includePersonalInfo;
        IncludeMedicalInfo = includeMedicalInfo;
        IncludeGpsLocation = includeGpsLocation;
        PicturePath = picturePath;
        DocumentPath = documentPath;
        SendEmail = contactMethods.Email;
        SendSms = contactMethods.Sms;
        SendMessenger = contactMethods.Messenger;
    }

    /// <summary>
    /// Gets a value indicating whether any file attachments are present.
    /// </summary>
    /// <returns>True if either a picture or document is attached; otherwise, false.</returns>
    /// <remarks>
    /// This property provides a quick way to check if the emergency has any
    /// file attachments (pictures or documents) that need to be processed
    /// or transmitted with the emergency notification.
    /// </remarks>
    public bool HasFileAttachments => !string.IsNullOrEmpty(PicturePath) || !string.IsNullOrEmpty(DocumentPath);

    /// <summary>
    /// Gets a value indicating whether any information attachments are enabled.
    /// </summary>
    /// <returns>True if any information attachment is enabled; otherwise, false.</returns>
    /// <remarks>
    /// This property provides a quick way to check if the emergency has any
    /// information attachments (personal info, medical info, GPS location)
    /// that should be included in the emergency notification.
    /// </remarks>
    public bool HasInformationAttachments => IncludePersonalInfo || IncludeMedicalInfo || IncludeGpsLocation;

    /// <summary>
    /// Gets a value indicating whether any contact methods are enabled.
    /// </summary>
    /// <returns>True if any contact method is enabled; otherwise, false.</returns>
    /// <remarks>
    /// This property provides a quick way to check if the emergency has any
    /// contact methods enabled for sending notifications. If no contact methods
    /// are enabled, the emergency notification may not be delivered.
    /// </remarks>
    public bool HasContactMethods => SendEmail || SendSms || SendMessenger;
}

public record WhichContactMethods
{
    public bool Email { get; set; }
    public bool Sms { get; set; }
    public bool Messenger { get; set; }
    public WhichContactMethods(bool email = true, bool sms = true, bool messenger = false)
    {
        Email = email;
        Sms = sms;
        Messenger = messenger;
    }
}
