<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  
  <div class="container">
    <h1 class="line">Корзина</h1>

    <div v-if="basket.length > 0">
      <BasketComponent :basket="basket" @refresh-basket="refreshBasket" />
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
  import { getBasket, placeAnOrder, deleteOrder } from '../api/ordersApi';
  import { getOrderElementByOrderId } from '../api/orderElementsApi';

  import BasketComponent from '../components/elements/BasketComponent.vue';

  export default {
    name: 'Basket',
    components: { BasketComponent },
    data() {
      return {
        myId: null,
        basketOrder: null,
        basket: []
      }
    },
    methods: {
      getMyData() {
        this.myId = localStorage.getItem('guid');
      },
      async getBasketNumber() {
        try {
          const response = await getBasket(this.myId);
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
          const response = await getOrderElementByOrderId(this.basketOrder.id);
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
          const response = await deleteOrder(this.basketOrder.id);
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
          const response = await placeAnOrder(this.basketOrder.id);
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
      warnInfo(message, error) {
        console.error(message, error);
        alert(error.message);
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

  p {
    font-size: 16px;
    line-height: 28px;
    padding: 0px 10px;
  }

  .line {
    background-image: linear-gradient(rgba(255, 255, 255, 0.1), rgba(255, 255, 255, 0.9), rgba(255, 255, 255, 0.1));
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
