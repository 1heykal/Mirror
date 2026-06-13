import { Routes } from '@angular/router';
import { NotesComponent } from './notes/notes.component';
import { CreateNoteComponent } from './create-note/create-note.component';

export const routes: Routes = [
  { path: '', component: NotesComponent },
  { path: 'notes', redirectTo: '', pathMatch: 'full' },
  { path: 'notes/create', component: CreateNoteComponent },
];
