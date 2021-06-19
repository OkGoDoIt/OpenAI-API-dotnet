namespace OpenAI_API.Dto
{
	public class OpenAiErrorDto
	{
		public OpenAiErrorDtoError Error { get; set; }

		public override string ToString()
		{
			return Error?.ToString();
		}
	}

	public class OpenAiErrorDtoError
	{
		public string Code { get; set; }
		public string Message { get; set; }

		public string Param { get; set; }
		public string Type { get; set; }

		public override string ToString()
		{
			return $"[Code = {Code}, Message = {Message}, Param = {Param}, Error type = {Type}";
		}
	}
}
