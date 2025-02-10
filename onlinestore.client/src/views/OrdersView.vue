<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <OrderEditWindow v-if="isOpenDialog" @close-dialog="clickWindowRedactor" :order="selectedOrder" />
  
  <div class="container">
    <h1 class="line">Заказы</h1>

    <div v-if="records.length > 0" class="table">
      <div class="row">
        <div class="cell colored">Заказ</div>
        <div class="cell colored">Заказчик</div>
        <div class="cell colored">Дата заказа</div>
        <div class="cell colored">Дата доставки</div>
        <div class="cell colored">Номер</div>
        <div class="cell colored">Статус</div>
        <div class="cell colored">Опции</div>
      </div>
      <div class="row" v-for="(record, index) in records" :key="index">
        <div class="cell base">[ {{ record.id }} ]</div>
        <div class="cell base">{{ record.customerName }} [ {{ record.customerId }} ]</div>
        <div class="cell base">{{ record.orderDate }}</div>
        <div class="cell base">{{ record.shipmentDate }}</div>
        <div class="cell base">{{ record.orderNumber }}</div>
        <div class="cell base">{{ record.orderStatus }}</div>
        <div class="cell base">
          <a class="accent" v-if="role === '1' || record.orderStatus === 'new'" @click="clickOnItem(index)">Редактировать</a>
          <a v-else>Нет</a>
        </div>
      </div>
    </div>
    <div v-else>
      <h2>Заказов нет.</h2>
    </div>

    <Pagination @page-changed="getOrders" :current="currentPage" :totalPages="totalPages" />

  </div>
</template>

<script>
  import { getPageOfOrders, getPageOfOrdersByCustomer } from '../api/ordersApi';

  import Pagination from '../components/pagination/PaginationComponent.vue'
  import OrderEditWindow from '../components/dialogs/OrderEditWindow.vue';

  export default {
    name: 'Orders',
    components: { Pagination, OrderEditWindow },
    data() {
      return {
        currentPage: 1,
        totalPages: 1,
        pageSize: 24,
        totalCount: 0,
        myId: null,
        role: null,
        records: [],
        selectedOrder: null,
        isOpenDialog: false
      }
    },
    methods: {
      getMyData() {
        this.myId = localStorage.getItem('guid');
        this.role = localStorage.getItem('role');
      },
      async getOrders(number = 1) {
        this.currentPage = number;
        try {
          const response = this.role === '1'
                        ? await getPageOfOrders(this.currentPage, this.pageSize)
                        : await getPageOfOrdersByCustomer(this.myId, this.currentPage, this.pageSize);
          if (response) {
            this.records = response.orderResponses;
            this.totalCount = response.totalCount;
            this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          } else {
            alert('Ошибка при получении заказов.');
          }
        } catch (error) {
          this.warnInfo('Ошибка при получении данных (Orders): ', error);
        }
      },
      async clickOnItem(index) {
        this.selectedOrder = this.records[index];
        this.isOpenDialog = true;
      },
      async clickWindowRedactor(state) {
        this.isOpenDialog = state;
        if (!state) {
          await this.getOrders();
        }
      },
      warnInfo(message, error) {
        console.error(message, error);
        alert('Проблемы с сервером!');
      }
    },
    mounted() {
      this.getMyData();
      this.getOrders();
    }
  }
</script>

<style scoped>
  .container {
    max-width: 65vw;
    margin: 0px auto;
    padding-top: 20px;
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
  }

  .colored {
    font-weight: bold;
    background-color: rgba(163, 194, 232, 1);
  }

  .base {
    background-color: white;
  }

  a, h1, h2, h3, p, .cell {
    font-weight: 500;
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

  .line {
    background-image: linear-gradient(rgba(255, 255, 255, 0.1), rgba(255, 255, 255, 0.9), rgba(255, 255, 255, 0.1));
  }
</style>
