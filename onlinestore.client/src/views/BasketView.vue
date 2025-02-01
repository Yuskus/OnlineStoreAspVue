<script>
  import axios from 'axios';

  export default {
    name: 'Basket',
    data() {
      return {
        myId: null,
        basket: []
      }
    },
    methods: {
      getMyData() {
        this.myId = localStorage.getItem('guid');
      },
      // дополнить информацией о каждом товаре, проверить имена переменных в template
      async getBasket() {
        try {
          const response = await axios.get(`http://localhost:5000/api/OrderElements/getbasket/${this.myId}`, {
            headers: {
              'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
          });
          
          if (response.status === 200 && response.data) {
            this.basket = response.data;
          } else {
            alert('Проблемы с сервером!');
            console.log("Статус ошибки: " + response.status);
          }
        } catch (error) {
          console.error('Ошибка при получении данных (Basket): ', error);
          alert('Проблемы с сервером!');
        }
      }
    },
    mounted() {
      //this.getBasket();
    }
  }
</script>

<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <div class="container">
    <h1 class="line">Корзина</h1>

    <div class="table">
      <div class="row colored">
        <div class="cell">Миниатюра</div>
        <div class="cell">Название</div>
        <div class="cell">Категория</div>
        <div class="cell">Количество</div>
        <div class="cell">Цена</div>
        <div class="cell">Опции</div>
      </div>
      <div class="row" v-for="(item, index) in basket" :key="index">
        <div class="cell">Image</div>
        <div class="cell">{{ item.id }}</div>
        <div class="cell">{{ item.name }}</div>
        <div class="cell">{{ item.category }}</div>
        <div class="cell">{{ item.price }}</div>
        <div class="cell">{{ item.code }}</div>
      </div>
    </div>

    <div class="options">
      <button><p>Очистить корзину</p></button>
      <button><p>Актуализация цен</p></button>
      <button><p>Оформить заказ</p></button>
    </div>

  </div>
</template>

<style scoped>
  .container {
    max-width: 65vw;
    margin: 0px auto;
    padding: 20px 0px 40px 0px;
    background-color: #e0ebf7;
    filter: drop-shadow(0 0.2rem 0.25rem rgba(0, 0, 0, 0.2));
  }

  .table {
    display: table;
    width: 100%;
    border-collapse: collapse;
  }

  .row {
    display: table-row;
  }

  .cell {
    display: table-cell;
    border: 1px solid #6678b1;
    padding: 10px;
    text-align: left;
    font-size: 18px;
    line-height: 28px;
    background-color: white;
  }

  .colored {
    font-weight: bold;
    background-color: rgba(163, 194, 232, 1);
  }

  h1, h3, p, .cell {
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-weight: 500;
    font-style: normal;
    color: #1c2633;
  }

  h1 {
    font-size: 26px;
    line-height: 36px;
    padding: 20px 10px;
  }

  h3 {
    font-size: 22px;
    line-height: 30px;
    padding: 15px 10px;
  }

  p {
    font-size: 18px;
    line-height: 28px;
    padding: 0px 10px;
  }

  button p {
    font-size: 16px;
  }

  .line {
    background-image: linear-gradient(rgba(255, 255, 255, 0.1), rgba(255, 255, 255, 0.9), rgba(255, 255, 255, 0.1));
  }

  hr {
    border: none;
    border-top: 1px solid rgba(0,0,0,0.08);
    margin-top: 20px;
  }

  .options {
    display: flex;
    justify-content: right;
    margin: 40px 10px 10px 10px;
  }

  button {
    min-height: 40px;
    margin: 0px 15px;
    padding: 5px;
    border-radius: 10px;
    border: 1px solid rgba(0,10,60,0.4);
    background-color: rgba(163, 194, 232, 1);
    transition: all 0.3s ease;
  }

  button:hover {
    background-color: rgba(133, 164, 202, 1);
  }
</style>
