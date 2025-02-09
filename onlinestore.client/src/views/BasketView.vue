<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <OrderElementEditWindow v-if="isOpenDialog === true" @close-dialog="clickWindowRedactor" :item="selectedItem" />
  
  <div class="container">
    <h1 class="line">Корзина</h1>

    <div v-if="basket.length > 0" class="table">
      <div class="row colored">
        <div class="cell">Миниатюра</div>
        <div class="cell">Название</div>
        <div class="cell">Категория</div>
        <div class="cell">Количество</div>
        <div class="cell">Цена за шт.</div>
        <div class="cell">Опции</div>
      </div>
      <div class="row" v-for="(item, index) in basket" :key="index" >
        <div class="cell">Image</div>
        <div class="cell">{{ item.itemResponse.name }}</div>
        <div class="cell">{{ item.itemResponse.category }}</div>
        <div class="cell">{{ item.itemsCount }}</div>
        <div class="cell">{{ item.itemResponse.price }}</div>
        <div class="cell"><a class="accent
          " @click="clickOnItem(index)">Редактировать</a></div>
      </div>
    </div>
    <div v-else>
      <h2>В корзине пусто.</h2>
    </div>

    <div class="options">
      <button @click="clearBasket()" ><p>Очистить корзину</p></button>
      <button @click="refreshBasket()" ><p>Актуализация цен</p></button>
      <button @click="placeAnOrder()"><p>Оформить заказ</p></button>
    </div>

  </div>
</template>

<script>
  import ordersApi from '../api/ordersApi';
  import orderElementsApi from '../api/orderElementsApi';
  import OrderElementEditWindow from '../components/OrderElementEditWindow.vue'

  export default {
    name: 'Basket',
    components: { OrderElementEditWindow },
    data() {
      return {
        myId: null,
        basketOrder: null,
        basket: [],
        isOpenDialog: false,
        selectedItem: null
      }
    },
    methods: {
      getMyData() {
        this.myId = localStorage.getItem('guid');
      },
      async getBasketNumber() {
        try {
          const response = await ordersApi.getBasket(this.myId);
          if (response) {
            this.basketOrder = response;
          } else {
            alert('Ошибка при получении информации о корзине.');
          }
        } catch (error) {
          this.warnInfo('Ошибка при получении данных (BasketView): ', error);
        }
      },
      async getBasketElements() {
        try {
          const response = await orderElementsApi.getOrderElementByOrderId(this.basketOrder.id);
          if (response) {
            this.basket = response;
          } else {
            alert('Ошибка при получении содержимого корзины.');
          }
        } catch (error) {
          this.warnInfo('Ошибка при получении данных (BasketView): ', error);
        }
      },
      async clearBasket() {
        try {
          const response = await ordersApi.deleteOrder(this.basketOrder.id);
          if (!response) {
            alert('Ошибка при очистке корзины.');
          }
          await this.refreshBasket();
        } catch (error) {
          this.warnInfo('Ошибка при удалении данных (BasketView): ', error);
        }
      },
      async placeAnOrder() {
        try {
          const response = await ordersApi.placeAnOrder(this.basketOrder.id);
          if (!response) {
            alert('Ошибка при очистке корзины.');
          }
          await this.refreshBasket();
        } catch (error) {
          this.warnInfo('Ошибка при изменении данных (Basket): ', error);
        }
      },
      async refreshBasket() {
        await this.getBasketNumber();
        await this.getBasketElements();
      },
      async clickOnItem(index) {
        this.selectedItem = this.basket[index];
        await this.clickWindowRedactor(true);
      },
      async clickWindowRedactor(state) {
        this.isOpenDialog = state;
        if (!state) {
          await this.refreshBasket();
        }
      },
      warnInfo(message, error) {
        console.error(message, error);
        alert('Проблемы с сервером!');
      }
    },
    mounted() {
      this.getMyData();
      this.refreshBasket();
    }
  }
</script>

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

  a, h1, h2, h3, p, .cell {
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-weight: 500;
    font-style: normal;
    color: #1c2633;
  }

  .accent {
    text-decoration: underline;
  }

  .accent:hover {
    color: #34547a;
  }

  h1 {
    font-size: 26px;
    line-height: 36px;
    padding: 20px 10px;
  }

  h2 {
    font-size: 24px;
    line-height: 32px;
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
