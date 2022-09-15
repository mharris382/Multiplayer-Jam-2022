using Godot;
using System;
using Game.core;

public class SteamSource : Node2D
{
	[Signal]
	delegate void SteamSourceChanged(Vector2 world_position, int output);
	
	[Export]
	private int _sourceOutput = 3;
	
	[Export]
	private bool _sourceStartsEnabled = true;
	private bool _sourceEnabled;


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
		if (_sourceEnabled)
		{
			RegisterSource();
			Debug.Log($"Registered source {Name} (rate={_sourceOutput}");
		}
		
		if (_sourceStartsEnabled){
			BroadcastSourceStateChanged();
		}
	}
	void BroadcastSourceStateChanged()
	{
		EmitSignal("SteamSourceChanged", Position, Enabled ? _sourceOutput : 0);
	}
	
	private void UnregisterSource()
	{
		GasStuff.Sources.Remove(this);
	}

	private void RegisterSource()
	{
		if (!GasStuff.Sources.Contains(this))
		{
			GasStuff.Sources.Add(this);
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

}
