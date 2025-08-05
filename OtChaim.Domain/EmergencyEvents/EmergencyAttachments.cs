using OtChaim.Domain.Common;

namespace OtChaim.Domain.EmergencyEvents;

/// <summary>
/// Represents attachments associated with an emergency.
/// </summary>
public class EmergencyAttachments : ValueObject
{
    /// <summary>
    /// Gets a value indicating whether a picture is attached.
    /// </summary>
    public bool HasPicture { get; }

    /// <summary>
    /// Gets a value indicating whether personal information is attached.
    /// </summary>
    public bool HasPersonalInfo { get; }

    /// <summary>
    /// Gets a value indicating whether medical information is attached.
    /// </summary>
    public bool HasMedicalInfo { get; }

    /// <summary>
    /// Gets a value indicating whether GPS location is attached.
    /// </summary>
    public bool HasGpsLocation { get; }

    /// <summary>
    /// Gets a value indicating whether a document is attached.
    /// </summary>
    public bool HasDocument { get; }

    /// <summary>
    /// Gets the picture file path if attached.
    /// </summary>
    public string? PicturePath { get; }

    /// <summary>
    /// Gets the document file path if attached.
    /// </summary>
    public string? DocumentPath { get; }

    /// <summary>
    /// Gets the GPS coordinates if attached.
    /// </summary>
    public Location? GpsLocation { get; }

    /// <summary>
    /// Gets the personal information content if attached.
    /// </summary>
    public string? PersonalInfoContent { get; }

    /// <summary>
    /// Gets the medical information content if attached.
    /// </summary>
    public string? MedicalInfoContent { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyAttachments"/> class.
    /// </summary>
    public EmergencyAttachments() { } // For EF Core

    /// <summary>
    /// Initializes a new instance of the <see cref="EmergencyAttachments"/> class.
    /// </summary>
    public EmergencyAttachments(
        bool hasPicture = false,
        bool hasPersonalInfo = false,
        bool hasMedicalInfo = false,
        bool hasGpsLocation = true,
        bool hasDocument = false,
        string? picturePath = null,
        string? documentPath = null,
        Location? gpsLocation = null,
        string? personalInfoContent = null,
        string? medicalInfoContent = null)
    {
        HasPicture = hasPicture;
        HasPersonalInfo = hasPersonalInfo;
        HasMedicalInfo = hasMedicalInfo;
        HasGpsLocation = hasGpsLocation;
        HasDocument = hasDocument;
        PicturePath = picturePath;
        DocumentPath = documentPath;
        GpsLocation = gpsLocation;
        PersonalInfoContent = personalInfoContent;
        MedicalInfoContent = medicalInfoContent;
    }

    /// <summary>
    /// Creates a copy of this EmergencyAttachments with updated values.
    /// </summary>
    public EmergencyAttachments With(
        bool? hasPicture = null,
        bool? hasPersonalInfo = null,
        bool? hasMedicalInfo = null,
        bool? hasGpsLocation = null,
        bool? hasDocument = null,
        string? picturePath = null,
        string? documentPath = null,
        Location? gpsLocation = null,
        string? personalInfoContent = null,
        string? medicalInfoContent = null)
    {
        return new EmergencyAttachments(
            hasPicture ?? HasPicture,
            hasPersonalInfo ?? HasPersonalInfo,
            hasMedicalInfo ?? HasMedicalInfo,
            hasGpsLocation ?? HasGpsLocation,
            hasDocument ?? HasDocument,
            picturePath ?? PicturePath,
            documentPath ?? DocumentPath,
            gpsLocation ?? GpsLocation,
            personalInfoContent ?? PersonalInfoContent,
            medicalInfoContent ?? MedicalInfoContent);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return HasPicture;
        yield return HasPersonalInfo;
        yield return HasMedicalInfo;
        yield return HasGpsLocation;
        yield return HasDocument;
        yield return PicturePath ?? string.Empty;
        yield return DocumentPath ?? string.Empty;
        yield return GpsLocation ?? Location.Empty;
        yield return PersonalInfoContent ?? string.Empty;
        yield return MedicalInfoContent ?? string.Empty;
    }
} 