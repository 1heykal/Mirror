import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HelloService } from '../services/hello/hello.service';
import { RouterModule } from '@angular/router';
import { NotesComponent } from './notes/notes.component';
import { CreateNoteComponent } from './create-note/create-note.component';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NotesComponent, CreateNoteComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'Mirror';
  helloService = inject(HelloService);
  hello: string = '';
  ngOnInit(): void {
    this.helloService.getHello().subscribe((hello) => {
      this.title = hello;
    });
  }
}
