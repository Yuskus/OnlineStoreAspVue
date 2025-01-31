<script>
  import Pagination from '../components/PaginationComponent.vue';
  import axios from 'axios';

  export default {
    name: 'Catalog',
    components: { Pagination },
    data() {
      return {
        items: [],
        currentPage: 1,
        totalPages: 1,
        pageSize: 12
      }
    },
    methods: {
      async getItems(number = 1) {
        this.currentPage = number;

        try {
          const response = await axios.get(`http://localhost:5000/api/Items/getpage?pageNumber=${this.currentPage}&pageSize=${this.pageSize}`, {
            headers: {
              'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
          });
          
          if (response.status === 200 && response.data) {
            this.items = response.data;
          } else {
            alert('Проблемы с сервером!');
            console.log("Статус ошибки: " + response.status);
          }
        } catch (error) {
          console.error('Ошибка при получении данных (Catalog): ', error);
          alert('Проблемы с сервером!');
        }
      }
    },
    mounted() {
      this.getItems();
    }
  }
</script>

<template>
  <div class="container">

    <div class="filters">
      <div class="button">Фильтр 1</div>
      <div class="button">Фильтр 2</div>
      <div class="button">Фильтр 3</div>
      <div class="button">Фильтр 4</div>
    </div>

    <div class="catalog">
      <div class="item" v-for="(item, index) in items" :key="index">
        <div><img src="../assets/item.jpg" /></div>
        <div class="item-desc">{{ item.name }}</div>
        <div class="item-desc">{{ item.category }}</div>
        <div class="item-desc">{{ item.price }}</div>
      </div>
    </div>

    <Pagination @page-changed="getItems" :current="currentPage" :totalPages="totalPages" />

  </div>
</template>

<style scoped>
  .container {
    max-width: 70vw;
    width: fit-content;
    margin: 0px auto;
    padding: 20px 0px 40px 0px;
  }

  .filters {
    display: block;
    justify-content: space-around;
    padding: 10px;
    align-items: center;
  }

  .button {
    display: inline-block;
    justify-content: space-around;
    text-decoration: none;
    margin: 5px 10px;
    padding: 10px;
    background-color: #f0f5fa;
    border-radius: 15px;
  }

  .button:hover {
    background-color: #dce1f0;
  }

  .catalog {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    padding: 20px 0px 30px 0px;
  }

  .item {
    background-color: #f0f5fa;
    align-items: center;
    box-shadow: 1px 2px 4px rgba(0, 50, 100, 0.2);
    align-content: space-around;
    margin: 10px;
    border-radius: 20px 20px 20px 20px;
  }

  img {
    border-radius: 20px 20px 0px 0px;
  }

  .item div {
    align-self: center;
  }

  .item-desc {
    padding: 10px;
  }

  .item:hover {
    box-shadow: 4px 8px 16px rgba(0, 50, 100, 0.4);
  }

  p {
    color: #1c2633;
  }
</style>

