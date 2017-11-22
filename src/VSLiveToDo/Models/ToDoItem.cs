using System;
using MvvmHelpers;

namespace VSLiveToDo.Models
{
    public class ToDoItem : ObservableObject
    {
        string text;
        public string Text { get => text; set => SetProperty(ref text, value); }

        string notes;
        public string Notes { get => notes; set => SetProperty(ref notes, value); }

        bool complete;
        public bool Complete { get => complete; set => SetProperty(ref complete, value); }

        public string Id { get; set; }
        public byte[] Version { get; set; }
    }
}
