using System.Collections;

public abstract class SFSOperation<T> : IEnumerator
{
	public object Current
	{
		get
		{
			return null;
		}
	}

	public abstract bool isDone { get; }

	public abstract float progress { get; }

	public abstract string error { get; }

	public abstract T result { get; }

	public bool MoveNext()
	{
		return !isDone;
	}

	public void Reset()
	{
	}
}
