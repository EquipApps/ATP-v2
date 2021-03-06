﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Windows.Media;

namespace EquipApps.WorkBench.Docking
{
	public abstract class DockViewModel : ReactiveObject
	{
		public DockViewModel()
		{
			Title = null;
			ContentId = null;
			IsSelected = false;
			IsActive = false;
		}

		[Reactive]
		public string Title
		{
			get;
			set;
		}

		public ImageSource IconSource { get; protected set; }

		[Reactive]
		public string ContentId
		{
			get;
			set;
		}

		[Reactive]
		public bool IsSelected
		{
			get;
			set;
		}

		[Reactive]
		public bool IsActive
		{
			get;
			set;
		}




		[Reactive]
		public bool CanFloat { get; set; } = true;
		[Reactive]
		public bool CanClose { get; set; } = true;
		[Reactive]
		public bool CanHide { get; set; } = true;
	}
}
