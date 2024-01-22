import { useEffect, useState } from "react";
import "./App.css";
import { Todo } from "./components/models/Todo";
const BASE_URL = "http://localhost:5014/api";

function App() {
  const [todos, setTodos] = useState<Todo[]>([]);

  const getTodos = async () => {
    const response = await fetch(BASE_URL + "/TodoApp/GetTodos").then(
      (response) => response.json()
    );
    setTodos(response);
  };

  useEffect(() => {
    getTodos();
  }, []);
  return (
    <>
      <div>hejsan todo app kommer här</div>
      {todos.map((todo) => (
        <p>
          <span>{todo.description}</span>
          <input type="checkbox" checked={todo.isChecked} />
        </p>
      ))}
    </>
  );
}

export default App;
