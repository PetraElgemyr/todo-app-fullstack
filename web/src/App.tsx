import { useEffect, useState } from "react";
import "./App.css";
import { Todo } from "./components/models/Todo";
const BASE_URL = "http://localhost:5014/api";

function App() {
  const [todos, setTodos] = useState<Todo[]>([]);

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
  return (
    <>
      <div>hejsan todo app kommer här</div>
      {todos.map((todo, key) => (
        <p key={key}>
          <span>{todo.Description}</span>
          <input
            type="checkbox"
            checked={todo.IsChecked}
            onChange={() => toggleIsChecked(todo.Id)}
          />
          <span>{todo.Id}</span>
        </p>
      ))}
    </>
  );
}

export default App;
