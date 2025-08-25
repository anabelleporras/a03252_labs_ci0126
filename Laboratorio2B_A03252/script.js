// script.js

// Bandera para alternar el color de fondo
let fondoAlterno = false;

function agregar() {
  const ul = document.getElementById('lista');
  const siguiente = ul.children.length + 1;
  const li = document.createElement('li');
  li.textContent = `Elemento${siguiente}`;
  ul.appendChild(li);
}

function cambiarFondo() {
  fondoAlterno = !fondoAlterno;
  document.body.style.backgroundColor = fondoAlterno ? '#f7e26b' : '#ffffff';
}

function borrar() {
  const ul = document.getElementById('lista');
  const ultimo = ul.lastElementChild;
  if (ultimo) {
    ul.removeChild(ultimo);
  } else {
    alert('La lista ya está vacía.');
  }
}
