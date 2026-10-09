public interface IWWWOperation<T>
{
	bool isDone { get; }

	float progress { get; }

	string error { get; }

	T result { get; }
}
