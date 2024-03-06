using MAGestionale.Services;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;


namespace MAGestionale.Security;

public static class Tools
{
	public static string ComputeMd5Hash(string data)
	{
		using MD5 md5HashProvider = MD5.Create();

		byte[] bytes = md5HashProvider.ComputeHash(Encoding.UTF8.GetBytes(data));
		StringBuilder builder = new StringBuilder();
		
		for (int i = 0; i < bytes.Length; i++)
		{
			builder.Append(bytes[i].ToString("x2"));
		}
		
		return builder.ToString();

	}
	public static bool EmailValidator(string email)
	{
		string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|it)$";
		return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
	}
	public static string PathToURL(string path)
	{
		try
		{
			Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
			MemoryStream ms = new MemoryStream();
			stream.CopyTo(ms);
			byte[] byteArray = ms.ToArray();
			var b64String = Convert.ToBase64String(byteArray);
			return "data:image/png;base64," + b64String;
		}
		catch
		{
		
		}
			return null;
	}
}
