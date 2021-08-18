﻿using JetBrains.Annotations;

 namespace DronDonDon.Resource.UI.Element.Progress
{
    public interface IProgressBar
    {
        [UsedImplicitly]
        int Progress { get; set; }
        [UsedImplicitly]
        float Speed { get; set; }
        [UsedImplicitly]
        bool Completed { get; }
        [CanBeNull]
        [UsedImplicitly]
        string Label { get; set; }
    }
}