import { Component, inject, Output, EventEmitter } from '@angular/core';
import { NotesService } from '../../services/notes/notes.service';
import { FormsModule } from '@angular/forms';
import { Note } from '../../Models/Note';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-create-note',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './create-note.component.html',
  styleUrl: './create-note.component.scss'
})
export class CreateNoteComponent {

  private notesService = inject(NotesService);
  note: Note = {
    id: 0,
    title: '',
    content: '',
    createdAt: new Date(),
    updatedAt: new Date()
  };

  @Output() noteCreated = new EventEmitter<Note>();

  SaveNote(){
    this.notesService.createNote(this.note).subscribe((note: Note) => {
      this.noteCreated.emit(note);
      this.note = this.emptyNote();
    });
  }

  private emptyNote(): Note {
    return { id: 0, title: '', content: '', createdAt: new Date(), updatedAt: new Date() };
  }
  
}
