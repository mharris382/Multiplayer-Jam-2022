using Godot;
using System;
using System.Collections.Generic;
using Game.blocks.gas;
using Game.core;

public interface ISteamSource
{
	int Output { get; set; }
	bool Enabled { get; set; }
	Vector2 Position { get; set; }
	Vector2 size { get; }
	void BroadcastSourceStateChanged();
	Vector2 GetWorldSpacePosition();
}

public interface ISteamSink
{
	int Demand { get; set; }
	Vector2 size { get; }
	Vector2 Position { get; set; }
	Vector2 pullSize { get; }
	Vector2 GetWorldSpacePosition();
	GridDirections GetPullDirections();
}
public class SteamSource : Node2D, ISteamSource
{
	
	[Signal]
	delegate void SteamSourceChanged(Vector2 world_position, int output);
	
	[Export()] public int _steamPixelSize = 16;
	[Export]
	private int _sourceOutput = 3;

	[Export()]
	private Vector2 _sourceShape = new Vector2(3, 3);

	[Export()]
	private Vector2 _sourceOffset = new Vector2(1, 1);
	
	[Export]
	private bool _sourceStartsEnabled = true;
	private bool _sourceEnabled;

	
	private List<LocalSource> _localSources = new List<LocalSource>();

	
	
	public int Output {
		get => Enabled ? _sourceOutput : 0;
		set {
			if(_sourceOutput != value){
				_sourceOutput = value;
				if(_sourceEnabled)
					BroadcastSourceStateChanged();
			}
		}
	}
	public bool Enabled{
		get => _sourceEnabled && _sourceOutput > 0;
		set {
			if (value != _sourceEnabled)
			{
				_sourceEnabled = value;
				BroadcastSourceStateChanged();
			}
		}
	}
	
	
	public override void _Ready()
	{
		_sourceEnabled = _sourceStartsEnabled;
		InitLocalSources();
		if (_sourceEnabled)
		{
			RegisterSource();
			Debug.Log($"Registered source {Name} (rate={_sourceOutput}");
		}
		
		if (_sourceStartsEnabled){
			BroadcastSourceStateChanged();
		}
	}

	private void InitLocalSources()
	{
		for (int i = 0; i < _sourceShape.x; i++)
		{
			for (int j = 0; j < _sourceShape.y; j++)
			{
				var newLocalSource = new LocalSource(this, new Vector2(i, j));
				_localSources.Add(newLocalSource);
			}
		}
	}

	public Vector2 size { get; } = new Vector2(1, 1);

	public void BroadcastSourceStateChanged()
	{
		EmitSignal("SteamSourceChanged", Position, Enabled ? _sourceOutput : 0);
	}
	
	private void UnregisterSource()
	{
		foreach (var source in _localSources)
		{
			GasStuff.RemoveSource(source);
		}
	}

	private void RegisterSource()
	{
		foreach (var source in _localSources)
		{
			GasStuff.AddSource(source);
		}
	}
	
	
	public void _on_button_down()
	{
		GD.Print("button callback triggered");
		Enabled = !Enabled;
	}


	public Vector2 GetWorldSpacePosition()
	{
		return GlobalPosition;
	}

	private struct LocalSource : ISteamSource
	{
		private SteamSource _source;
		private readonly Vector2 _offset;

		private bool _enabled;
		private int _output;
		public int Output
		{
			get => _source.Output;
			set => _output = value;
		}

		public bool Enabled
		{
			get => _source.Enabled && _enabled;
			set => _enabled = value;
		}

		public Vector2 Position
		{
			get => _source.Position + (_offset ) + (_source._sourceOffset * _source._steamPixelSize);
			set { }
		}

		public Vector2 size { get; }
		public void BroadcastSourceStateChanged() { }

		public Vector2 GetWorldSpacePosition()
		{
			return Position;
		}

		public LocalSource(SteamSource parent, Vector2 offset)
		{
			size = Vector2.One;
			this._offset = offset* parent._steamPixelSize;
			_source = parent;
			_enabled = parent.Enabled;
			_output = parent.Output;
		}
	}
}
