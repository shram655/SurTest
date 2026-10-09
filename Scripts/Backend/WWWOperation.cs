using System;
using System.Collections;

public abstract class WWWOperation<T> : IEnumerator, IDisposable, IWWWOperation<T>
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

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected abstract void Dispose(bool disposing);

	~WWWOperation()
	{
		Dispose(false);
	}
}
