<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <h2>Danh sách khách hàng thành viên</h2>
                <v-layout row nowrap>
                    <v-text-field v-model="searchParamsNguoiDung.keywords"
                                  @input="getDataFromApi(searchParamsNguoiDung)"
                                  hide-details
                                  append-icon="search"
                                  label="Tìm kiếm"
                                  placeholder="Tìm kiếm theo tên, địa chỉ, số điện thoại..."
                                  class="mb-2 ml-1"></v-text-field>
                    <v-spacer></v-spacer>
                    <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" style="margin-top: 15px" dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsNguoiDung"
                                      :loading="loadingTable"
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsNguoiDung"
                                      :total-items="searchParamsNguoiDung.totalItems" 
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-xs-center text-no-wrap">
                                    {{props.index+1}}
                                </td>
                                <td class="text-xs-center" style="width:200px">{{ props.item.TenNguoiDung }}</td>
                                <td class="text-xs-center" style="width:200px">{{ props.item.SoDienThoai }}</td>
                                <td class="text-xs-center" style="width:200px">{{ props.item.DiemThuong }}</td>
                                <td>{{ props.item.DiaChi }}</td>
                                <td class="text-xs-center text-no-wrap" style="width:80px;">
                                    <v-btn icon small class="mx-0" @click="showModalThemSua(true,props.item)">
                                        <v-icon small color="teal">edit</v-icon>
                                    </v-btn>
                                    <v-btn icon small class="mx-0" @click="confirmDelete(props.item)">
                                        <v-icon small color="pink">delete</v-icon>
                                    </v-btn>
                                </td>
                            </template>
                        </v-data-table>
                    </v-flex>
                </v-layout>
            </v-card-text>
        </v-card>
        <v-dialog v-model="dialogConfirmDelete" max-width="290">
            <v-card>
                <v-card-title class="headline">Xác nhận xóa</v-card-title>
                <v-card-text class="pt-3">Bạn có chắc chắn muốn xóa?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click.native="dialogConfirmDelete=false" flat>Hủy</v-btn>
                    <v-btn color="red darken-1" @click.native="deleteNguoiDung" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-nguoi-dung ref="themSuaNguoiDung" @save='getDataFromApi(searchParamsNguoiDung)'></them-sua-nguoi-dung>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import NguoiDungApi, { NguoiDungApiSearchParams } from '@/apiResources/NguoiDungApi';
    import { NguoiDung } from '@/models/NguoiDung';
    import ThemSuaNguoiDung from './ThemSuaNguoiDung.vue';

    export default Vue.extend({
        components: {
            ThemSuaNguoiDung
        },
        data() {
            return {
                dsNguoiDung: [] as NguoiDung[],
                tableHeader: [
                    { text: 'STT', value: '#', align: 'center', sortable: false },
                    { text: 'Tên người dùng', value: 'TenNguoiDung', align: 'center', sortable: true },
                    { text: 'Số điện thoại', value: 'SoDienThoai', align: 'center', sortable: false },
                    { text: 'Điểm thưởng', value: 'DiemThuong', align: 'center', sortable: true },
                    { text: 'Địa chỉ', value: 'DiaChi', align: 'center', sortable: false },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsNguoiDung: { includeEntities: true, rowsPerPage: 10 } as NguoiDungApiSearchParams,
                loadingTable: false,
                selectedNguoiDung: {} as NguoiDung,
                dialogConfirmDelete: false,
                search: "",
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsNguoiDung);
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaNguoiDung as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsNguoiDung: NguoiDungApiSearchParams): void {
                this.loadingTable = true;
                NguoiDungApi.search(searchParamsNguoiDung).then(res => {
                    this.dsNguoiDung = res.Data;
                    this.searchParamsNguoiDung.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            confirmDelete(nguoiDung: NguoiDung): void {
                this.selectedNguoiDung = nguoiDung;
                this.dialogConfirmDelete = true;
            },
            deleteNguoiDung(): void {
                NguoiDungApi.delete(this.selectedNguoiDung.NguoiDungID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsNguoiDung);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

