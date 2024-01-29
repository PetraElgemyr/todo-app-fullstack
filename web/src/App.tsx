import { useEffect, useState } from "react";
import "./App.css";
import { Todo } from "./components/models/Todo";
const BASE_URL = "http://localhost:5014/api";

function App() {
  const [todos, setTodos] = useState<Todo[]>([]);
  const [newInput, setNewInput] = useState<string>("");
  const getTodos = async () => {
    try {
      const response = await fetch(BASE_URL + "/TodoApp/GetTodos").then(
        (response) => response.json()
      );
      console.log(response);

      setTodos(response);
    } catch (error) {
      return error;
    }
  };

  useEffect(() => {
    getTodos();
  }, []);

  //localhost:5014/api/TodoApp/UpdateTodo?id=1
  const toggleIsChecked = async (id: number) => {
    await fetch(BASE_URL + "/TodoApp/UpdateTodo?id=" + id, {
      method: "PUT",
    });
    getTodos();
  };

  const deleteTodo = async (id: number) => {
    await fetch(BASE_URL + "/TodoApp/DeleteTodo?id=" + id, {
      method: "DELETE",
    });
    getTodos();
  };

  const handleOnChangeInput = (input: string) => {
    setNewInput(input);
  };

  const addTodo = async (description: string) => {
    await fetch(BASE_URL + "/TodoApp/AddTodo", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ Description: description, IsChecked: false }),
    });
    setNewInput("");
    getTodos();
  };

  return (
    <>
      <div>hejsan todo app kommer här</div>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          addTodo(newInput);
        }}
      >
        <input
          type="text"
          placeholder="Skriv in en todo"
          value={newInput}
          onChange={(e) => handleOnChangeInput(e.target.value)}
        />
        <button>Lägg till</button>
      </form>

      {todos.map((todo, key) => (
        <p key={key}>
          <span>{todo.Description}</span>
          <input
            type="checkbox"
            checked={todo.IsChecked}
            onChange={() => toggleIsChecked(todo.Id)}
          />
          <button onClick={() => deleteTodo(todo.Id)}>Ta bort</button>
        </p>
      ))}
    </>
  );
}

export default App;
