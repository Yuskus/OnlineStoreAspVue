<template>
    <div>
        <div class="table" v-if="basket.length > 0">
            <div class="row">
                <div v-for="(name, index) in columnsNames" :key="index" class="cell colored">{{ name }}</div>
            </div>
            <div class="row" v-for="(item, index) in basket" :key="index" >
                <div class="cell"><img src="../../assets/item.jpg" width="80" height="80" /></div>
                <div class="cell">{{ item.itemResponse.name }}</div>
                <div class="cell">{{ item.itemResponse.category }}</div>
                <div class="cell">
                  <div v-if="immutable">
                    {{ item.itemsCount }}
                  </div>
                  <div v-else>
                    <QuantityRegulator v-model="item.itemsCount" :index="index" @update-request="updateQuantity" />
                  </div>
                </div>
                <div class="cell">{{ item.itemResponse.price }}</div>
                <div class="cell">{{ item.itemResponse.price * item.itemsCount }}</div>
                <div v-if="immutable" class="cell">Нет</div>
                <div v-else class="cell"><a class="accent" @click="deleteItem(index)">Удалить</a></div>
            </div>
        </div>
        <div v-else>
          <h2>В корзине пусто.</h2>
        </div>
    </div>
</template>

<script>
  import { updateOrderElement, deleteOrderElement } from '../../api/orderElementsApi';

  import QuantityRegulator from '../forms/QuantityRegulator.vue';

  export default {
    components: { QuantityRegulator },
    props: {
        basket: {
          type: Array,
          required: true
        },
        immutable: {
          type: Boolean,
          default: false
        }
    },
    data() {
      return {
        columnsNames: [ 'Миниатюра', 'Название', 'Категория', 'Количество', 'Цена за шт.', 'Суммарно', 'Опции' ]
      }
    },
    methods: {
      async deleteItem(index) {
        try {
          const response = await deleteOrderElement(this.basket[index].id);
          if (!response) {
            alert('Ошибка при удалении элемента заказа (OrderElement).');
          }
        } catch (error) {
          this.warnInfo('Ошибка при удалении данных (BasketComponent): ', error);
        } finally {
          this.$emit('refresh-basket');
        }
      },
      async updateQuantity(index) {
        try {
          const orderElement = this.basket[index];
          const response = await updateOrderElement(orderElement.id, orderElement);
          if (!response) {
            alert('Ошибка при изменении элемента заказа (OrderElement).');
          }
        } catch (error) {
          this.warnInfo('Ошибка при изменении данных (BasketComponent): ', error);
        }
      },
      warnInfo(message, error) {
        console.error(message, error);
        alert(error.message);
      }
    }
  }
</script>

<style scoped>
  * {
    font-weight: 500;
    color: #1c2633;
  }

  h2 {
    font-size: 24px;
    line-height: 32px;
    padding: 20px 10px;
    text-align: center;
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
    border-top: 1px solid #6678b1;
    border-bottom: 1px solid #6678b1;
    padding: 10px;
    text-align: left;
    font-size: 18px;
    background-color: white;
    vertical-align: middle;
  }

  .colored {
    font-weight: bold;
    background-color: rgba(163, 194, 232, 1);
  }

  .accent {
    text-decoration: underline;
  }

  .accent:hover {
    color: #34547a;
  }
</style>