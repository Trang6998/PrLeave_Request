<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <h2>Feedback khách hàng</h2>
                <v-layout row nowrap>
                    <v-text-field v-model="searchParamsLienHe.keywords"
                                  @input="getDataFromApi(searchParamsLienHe)"
                                  hide-details
                                  append-icon="search"
                                  label="Tìm kiếm"
                                  placeholder="Tìm kiếm theo tên, số điện thoại..."
                                  class="mb-2 ml-1"></v-text-field>
                    <v-spacer></v-spacer>
                    <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" style="margin-top: 15px" dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsLienHe"
                                      :loading="loadingTable"
                                      :search="search"
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-md-center" style="width:20px">{{props.index + 1}}</td>
                                <td class="text-md-center" style="width:200px">{{ props.item.HoTen }}</td>
                                <td class="text-md-center" style="width:200px">{{ props.item.SoDienThoai }}</td>
                                <td>{{ props.item.NoiDung }}</td>
                                <td class="text-md-center" style="width:80px;">
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
                    <v-btn color="red darken-1" @click.native="deleteLienHe" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-lien-he ref="themSuaLienHe" @save='getDataFromApi(searchParamsLienHe)'></them-sua-lien-he>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import LienHeApi, { LienHeApiSearchParams } from '@/apiResources/LienHeApi';
    import { LienHe } from '@/models/LienHe';
    import ThemSuaLienHe from './ThemSuaLienHe.vue';

    export default Vue.extend({
        components: {
            ThemSuaLienHe
        },
        data() {
            return {
                dsLienHe: [] as LienHe[],
                tableHeader: [
                    { text: 'STT', align: 'center', sortable: false },
                    { text: 'Tên khách hàng', value: 'NguoiDung === null ? "" : NguoiDung.TenNguoiDung', align: 'center', sortable: true },
                    { text: 'Số điện thoại', value: 'NguoiDung === null ? "" : NguoiDung.TenNguoiDung', align: 'center', sortable: true },
                    { text: 'Nội dung', value: 'NoiDung', align: 'center', sortable: false },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsLienHe: { includeEntities: true, rowsPerPage: 10 } as LienHeApiSearchParams,
                loadingTable: false,
                selectedLienHe: {} as LienHe,
                dialogConfirmDelete: false,
                search: "",
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsLienHe);
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaLienHe as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsLienHe: LienHeApiSearchParams): void {
                this.loadingTable = true;
                LienHeApi.search(searchParamsLienHe).then(res => {
                    this.dsLienHe = res.Data;
                    this.searchParamsLienHe.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            confirmDelete(lienHe: LienHe): void {
                this.selectedLienHe = lienHe;
                this.dialogConfirmDelete = true;
            },
            deleteLienHe(): void {
                LienHeApi.delete(this.selectedLienHe.LienHeID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsLienHe);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

