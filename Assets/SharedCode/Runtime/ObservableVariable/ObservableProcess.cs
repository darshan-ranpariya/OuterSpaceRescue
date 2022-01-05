using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableProcess
{
    public enum Status { NotStarted, InProcess, Completed, Failed }

    public Status status = Status.NotStarted;
    public string statusMessage = string.Empty;
    public object dataObject = null;
    public Action Updated;

    public void UpdateProcess(Status _status)
    {
        UpdateProcess(_status, statusMessage, dataObject);
    }

    public void UpdateProcess(Status _status, string _statusMessage)
    {
        UpdateProcess(_status, _statusMessage, dataObject);
    }

    public void UpdateProcess(Status _status, string _statusMessage, object _dataObject)
    {
        if (Updated != null) Updated();
    }
}