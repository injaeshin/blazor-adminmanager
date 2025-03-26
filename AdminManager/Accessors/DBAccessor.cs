using Dapper;
using System.Data;
using AdminManager.Model;
using AdminManager.Common;
using AdminManager.ModelView;
using Radzen.Blazor.Rendering;
using Quartz.Impl.AdoJobStore;

namespace AdminManager.Accessors
{
    public interface IDBAccessor
    {
        Task<long> GetAuidByAccountNameAsync(string name);
        Task<long> GetAuidByCharacterNameAsync(string name);

        Task<AccountModel> GetAccountAsync(long auid);
        Task<AccountTierModel> GetTierAsync(long auid);

        Task<IEnumerable<CharacterModel>> GetCharactersAsync(long auid);
        Task<CharacterDetailModel> GetCharacterAsync(string name);
        Task<CharacterDetailModel> GetCharacterAsync(long cuid);
        
        Task<IEnumerable<ItemModel>> GetItemAsync(long cuid);
        Task<IEnumerable<CurrencyModel>> GetCurrencyAsync(long cuid);

        Task<IEnumerable<MailModel>> GetMailAsync(long auid, long cuid);
        Task<IEnumerable<MailAttachItemModel>> GetMailItemByMuid(bool isAccount, long muid);

        Task<IEnumerable<QuestModel>> GetQuestAsync(long cuid);
        Task<IEnumerable<QuestTaskModel>> GetQuestTaskAsync(long cuid);

        Task<bool> AddSystemMessageAsync(SystemMessageModel systemMessage);
        Task<bool> AddMessageAsync(MessageModel mms);
        Task<List<SystemMessageModel>> GetSystemMessageAsync();
        Task<List<MessageModel>> GetMessageAsync(int no);
        Task<bool> RemoveMessageScheduleAsync(int no);
        Task<bool> RemoveMessageAsync(int no);

        Task<bool> AddAdminMailAsync(int mailId, long begin, long end, List<AdminMailAttachItemModel> items);
        Task<bool> AddAdminMailToUserAsync(long auid, int mailId, long begin, long end, List<AdminMailAttachItemModel> items);
        Task<bool> AddAdminMailToCharacterAsync(long cuid, int mailId, long begin, long end, List<AdminMailAttachItemModel> items);
        Task<bool> RemoveAdminMailAsync(long amuid);
        Task<bool> RemoveAccountAdminMailAsync(long amuid);
        Task<bool> RemoveCharacterAdminMailAsync(long amuid);

        Task<List<AdminMailModel>> GetAdminMailAsync();
        Task<List<AdminMailAttachItemModel>> GetAdminMailAttachItem(long amuid);
        Task<List<AdminMailModel>> GetAdminAccountMailAsync();
        Task<List<AdminMailAttachItemModel>> GetAdminAccountMailAttachItemAsync(long amuid);
        Task<List<AdminMailModel>> GetAdminCharacterMailAsync();
        Task<List<AdminMailAttachItemModel>> GetAdminCharacterMailAttachItemAsync(long amuid);


        Task<bool> IsExistsAuid(long auid);

        // airdrop
        Task<long> GetAirdropCollectAmount(int serverId, GameType gameType);
        Task<List<AirdropEventModel>> GetAirdropEventAsync();
        Task<bool> AddAirdropEventAsync(AirdropEventModel aes);
        Task<bool> RemoveAirdropEventAsync(int no);
        Task<bool> SetAirdropEventResultAsync(int no, EventResultType eventRet, string msg);
    }

    public class DbAccessor : IDBAccessor
    {
        private readonly DapperProvider _dapperProvider;

        public DbAccessor(DapperProvider dp)
        {
            _dapperProvider = dp;

            Init();
        }

        private void Init()
        {
            AccountModelMapper.Init();
            CharacterModelMapper.Init();
            MessageModelMapper.Init();
            MailModelMapper.Init();
            AdminMailModelMapper.Init();
            ItemModelMapper.Init();
            CurrencyModelMapper.Init();
            QuestModelMapper.Init();
            EventModelMapper.Init();
        }

        private IDbConnection OpenAdminDB() => _dapperProvider.Open(DapperProvider.DB.Admin);
        private IDbConnection OpenUserDB() => _dapperProvider.Open(DapperProvider.DB.User);
        private IDbConnection OpenGameDB() => _dapperProvider.Open(DapperProvider.DB.Game);

        public async Task<long> GetAuidByAccountNameAsync(string name)
        {
            const string cmd = "tp_auid_by_name_select";
            var param = new DynamicParameters();
            param.Add("_name", name);

            using var conn = OpenUserDB();
            return await conn.QueryFirstOrDefaultAsync<long>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<AccountModel> GetAccountAsync(long auid)
        {
            const string cmd = "tp_account_by_auid_select";
            var param = new DynamicParameters();
            param.Add("_auid", auid);

            using var conn = OpenUserDB();
            return await conn.QueryFirstOrDefaultAsync<AccountModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> GetAuidByCharacterNameAsync(string name)
        {
            const string cmd = "tp_auid_by_char_name_select";
            var param = new DynamicParameters();
            param.Add("_name", name);

            using var conn = OpenGameDB();
            return await conn.QueryFirstOrDefaultAsync<long>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<AccountTierModel> GetTierAsync(long auid)
        {
            const string cmd = "tp_tier_by_auid_select";
            var param = new DynamicParameters();
            param.Add("_auid", auid);

            using var conn = OpenGameDB();
            return await conn.QueryFirstOrDefaultAsync<AccountTierModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CharacterModel>> GetCharactersAsync(long auid)
        {
            const string cmd = "tp_chars_by_auid_select";
            var param = new DynamicParameters();
            param.Add("_auid", auid);

            using var conn = OpenGameDB();
            return await conn.QueryAsync<CharacterModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<CharacterDetailModel> GetCharacterAsync(long cuid)
        {
            const string cmd = "tp_char_by_cuid_select";
            var param = new DynamicParameters();
            param.Add("_cuid", cuid);

            using var conn = OpenGameDB();
            return await conn.QuerySingleOrDefaultAsync<CharacterDetailModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<CharacterDetailModel> GetCharacterAsync(string name)
        {
            const string cmd = "tp_char_by_name_select";
            var param = new DynamicParameters();
            param.Add("_name", name);

            using var conn = OpenGameDB();
            return await conn.QuerySingleOrDefaultAsync<CharacterDetailModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CurrencyModel>> GetCurrencyAsync(long cuid)
        {
            const string cmd = "tp_currency_by_cuid_select";
            var param = new DynamicParameters();
            param.Add("_cuid", cuid);

            using var conn = OpenGameDB();
            return await conn.QueryAsync<CurrencyModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ItemModel>> GetItemAsync(long cuid)
        {
            var items = new List<ItemModel>();

            const string cmd1 = "tp_item_using_by_cuid_select";
            const string cmd2 = "tp_item_equip_by_cuid_select";
            var param = new DynamicParameters();
            param.Add("_cuid", cuid);

            using var conn = OpenGameDB();
            items.AddRange(await conn.QueryAsync<ItemModel>(cmd1, param, commandType: CommandType.StoredProcedure));
            items.AddRange(await conn.QueryAsync<ItemModel>(cmd2, param, commandType: CommandType.StoredProcedure));

            return items;
        }

        #region Mail
        public async Task<IEnumerable<MailModel>> GetMailAsync(long auid, long cuid)
        {
            const string cmd = "tp_mail_by_cuid_select";
            var param = new DynamicParameters();
            param.Add("_auid", auid);
            param.Add("_cuid", cuid);

            using var conn = OpenGameDB();
            return await conn.QueryAsync<MailModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<MailAttachItemModel>> GetMailItemByMuid(bool isAccount, long muid)
        {
            const string cmd = "tp_mail_reward_by_mail_uid_select";
            var param = new DynamicParameters();
            param.Add("_is_account", isAccount);
            param.Add("_mail_uid", muid);

            using var conn = OpenGameDB();
            return await conn.QueryAsync<MailAttachItemModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region Quest
        public async Task<IEnumerable<QuestModel>> GetQuestAsync(long cuid)
        {
            const string cmd = "tp_quest_by_cuid_select";
            var param = new DynamicParameters();
            param.Add("_cuid", cuid);

            using var conn = OpenGameDB();
            return await conn.QueryAsync<QuestModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<QuestTaskModel>> GetQuestTaskAsync(long cuid)
        {
            const string cmd = "tp_quest_tasks_by_cuid_select";
            var param = new DynamicParameters();
            param.Add("_cuid", cuid);

            using var conn = OpenGameDB();
            return await conn.QueryAsync<QuestTaskModel>(cmd, param, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region SystemMessage
        public async Task<bool> AddSystemMessageAsync(SystemMessageModel msm)
        {
            const string cmd = "tp_system_message_upsert";
            var param = new DynamicParameters();
            param.Add("_no", msm.No);
            param.Add("_begin_ts", msm.BeginTimeStamp);
            param.Add("_end_ts", msm.BeginTimeStamp);
            param.Add("_interval_min", msm.Interval);

            using var conn = OpenAdminDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<bool> AddMessageAsync(MessageModel mm)
        {
            const string cmd = "tp_message_upsert";
            using var conn = OpenAdminDB();

            var param = new DynamicParameters();
            param.Add("_no", mm.No);
            param.Add("_lang", mm.Language);
            param.Add("_msg", mm.Message);

            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<List<SystemMessageModel>> GetSystemMessageAsync()
        {
            const string cmd = "tp_system_message_select";
            var param = new DynamicParameters();

            using var conn = OpenAdminDB();
            var ret = await conn.QueryAsync<SystemMessageModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<List<MessageModel>> GetMessageAsync(int no)
        {
            const string cmd = "tp_message_select";
            var param = new DynamicParameters();
            param.Add("_no", no);

            using var conn = OpenAdminDB();
            var ret = await conn.QueryAsync<MessageModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<bool> RemoveMessageScheduleAsync(int no)
        {
            const string cmd = "tp_system_message_delete";
            var param = new DynamicParameters();
            param.Add("_no", no);

            using var conn = OpenAdminDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<bool> RemoveMessageAsync(int no)
        {
            const string cmd = "tp_message_delete";
            var param = new DynamicParameters();
            param.Add("_no", no);

            using var conn = OpenAdminDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }
        #endregion

        #region AdminMail
        public async Task<bool> AddAdminMailAsync(int mailId, long begin, long end, List<AdminMailAttachItemModel> items)
        {
            using var conn = OpenGameDB();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                const string cmd = "tp_admin_mail_all_insert";

                var param = new DynamicParameters();
                param.Add("_mail_id", mailId);
                param.Add("_begin_ts", begin);
                param.Add("_end_ts", end);

                var taskRet = await conn.QueryAsync<dynamic>(cmd, param, commandType: CommandType.StoredProcedure, transaction: tran);
                var ret = taskRet.FirstOrDefault();
                if (ret.NewAMUID == 0)
                {
                    tran.Rollback();
                    return false;
                }

                var amuid = ret.NewAMUID;
                foreach (var (v, i) in items.Select((v, i) => (v, i)))
                {
                    const string subCmd = "tp_admin_mail_all_reward_insert";

                    var subParam = new DynamicParameters();
                    subParam.Add("_amuid", amuid);
                    subParam.Add("_idx", i);
                    subParam.Add("_type", v.ItemType);
                    subParam.Add("_param", v.Param);
                    subParam.Add("_amount", v.Amount);

                    var subRet = await conn.ExecuteAsync(subCmd, subParam, commandType: CommandType.StoredProcedure, transaction: tran);
                    if (subRet == 0)
                    {
                        tran.Rollback();
                        return false;
                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }
        public async Task<bool> AddAdminMailToUserAsync(long auid, int mailId, long begin, long end, List<AdminMailAttachItemModel> items)
        {
            using var conn = OpenGameDB();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                const string cmd = "tp_admin_mail_target_a_insert";

                var param = new DynamicParameters();
                param.Add("_auid", auid);
                param.Add("_mail_id", mailId);
                param.Add("_begin_ts", begin);
                param.Add("_end_ts", end);

                var ret = await conn.QueryFirstOrDefaultAsync<dynamic>(cmd, param, commandType: CommandType.StoredProcedure, transaction: tran);
                if (ret.NewAMUID == 0)
                {
                    tran.Rollback();
                    return false;
                }

                var amuid = ret.NewAMUID;
                foreach (var (v, i) in items.Select((v, i) => (v, i)))
                {
                    const string subCmd = "tp_admin_mail_target_a_reward_insert";

                    var subParam = new DynamicParameters();
                    subParam.Add("_amuid", amuid);
                    subParam.Add("_idx", i);
                    subParam.Add("_type", v.ItemType);
                    subParam.Add("_param", v.Param);
                    subParam.Add("_amount", v.Amount);

                    var subRet = await conn.ExecuteAsync(subCmd, subParam, commandType: CommandType.StoredProcedure, transaction: tran);
                    if (subRet == 0)
                    {
                        tran.Rollback();
                        return false;
                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        //public async Task<bool> AddAdminMailToAccountAsync(long auid, int mailId, long begin, long end, List<AdminMailAttachItemModel> items)
        //{
        //    using var conn = OpenGameDB();
        //    conn.Open();
        //    using var tran = conn.BeginTransaction();
        //    try
        //    {
        //        const string cmd = "tp_admin_mail_account_insert";

        //        var param = new DynamicParameters();
        //        param.Add("_auid", auid);
        //        param.Add("_mail_id", mailId);
        //        param.Add("_begin_ts", begin);
        //        param.Add("_end_ts", end);

        //        var ret = await conn.QueryFirstOrDefaultAsync<dynamic>(cmd, param, commandType: CommandType.StoredProcedure, transaction: tran);
        //        if (ret.NewAMUID == 0)
        //        {
        //            tran.Rollback();
        //            return false;
        //        }

        //        var amuid = ret.NewAMUID;
        //        foreach (var (v, i) in items.Select((v, i) => (v, i)))
        //        {
        //            const string subCmd = "tp_admin_mail_account_reward_insert";

        //            var subParam = new DynamicParameters();
        //            subParam.Add("_amuid", amuid);
        //            subParam.Add("_idx", i);
        //            subParam.Add("_type", v.ItemType);
        //            subParam.Add("_param", v.Param);
        //            subParam.Add("_amount", v.Amount);

        //            var subRet = await conn.ExecuteAsync(subCmd, subParam, commandType: CommandType.StoredProcedure, transaction: tran);
        //            if (subRet == 0)
        //            {
        //                tran.Rollback();
        //                return false;
        //            }
        //        }

        //        tran.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        Console.WriteLine(ex.ToString());
        //        return false;
        //    }

        //    return true;
        //}

        public async Task<bool> AddAdminMailToCharacterAsync(long cuid, int mailId, long begin, long end, List<AdminMailAttachItemModel> items)
        {
            using var conn = OpenGameDB();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                const string cmd = "tp_admin_mail_target_c_insert";

                var param = new DynamicParameters();
                param.Add("_cuid", cuid);
                param.Add("_mail_id", mailId);
                param.Add("_begin_ts", begin);
                param.Add("_end_ts", end);

                var ret = await conn.QueryFirstOrDefaultAsync<dynamic>(cmd, param, commandType: CommandType.StoredProcedure, transaction: tran);
                if (ret.NewAMUID == 0)
                {
                    tran.Rollback();
                    return false;
                }

                var amuid = ret.NewAMUID;
                foreach (var (v, i) in items.Select((v, i) => (v, i)))
                {
                    const string subCmd = "tp_admin_mail_target_c_reward_insert";

                    var subParam = new DynamicParameters();
                    subParam.Add("_amuid", amuid);
                    subParam.Add("_idx", i);
                    subParam.Add("_type", v.ItemType);
                    subParam.Add("_param", v.Param);
                    subParam.Add("_amount", v.Amount);

                    var subRet = await conn.ExecuteAsync(subCmd, subParam, commandType: CommandType.StoredProcedure, transaction: tran);
                    if (subRet == 0)
                    {
                        tran.Rollback();
                        return false;
                    }
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveAdminMailAsync(long amuid)
        {
            const string cmd = "tp_admin_mail_all_by_amuid_delete";
            var param = new DynamicParameters();
            param.Add("_amuid", amuid);

            using var conn = OpenGameDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<bool> RemoveAccountAdminMailAsync(long amuid)
        {
            const string cmd = "tp_admin_mail_target_a_by_amuid_delete";
            var param = new DynamicParameters();
            param.Add("_amuid", amuid);

            using var conn = OpenGameDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<bool> RemoveCharacterAdminMailAsync(long amuid)
        {
            const string cmd = "tp_admin_mail_target_c_by_amuid_delete";
            var param = new DynamicParameters();
            param.Add("_amuid", amuid);

            using var conn = OpenGameDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<List<AdminMailModel>> GetAdminMailAsync()
        {
            const string cmd = "tp_admin_mail_all_select";
            var param = new DynamicParameters();

            using var conn = OpenGameDB();
            var ret = await conn.QueryAsync<AdminMailModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<List<AdminMailAttachItemModel>> GetAdminMailAttachItem(long amuid)
        {
            const string cmd = "tp_admin_mail_all_reward_by_amuid_select";
            var param = new DynamicParameters();
            param.Add("_amuid", amuid);

            using var conn = OpenGameDB();
            var ret = await conn.QueryAsync<AdminMailAttachItemModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<List<AdminMailModel>> GetAdminAccountMailAsync()
        {
            const string cmd = "tp_admin_mail_target_a_select";
            var param = new DynamicParameters();

            using var conn = OpenGameDB();
            var ret = await conn.QueryAsync<AdminMailModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<List<AdminMailAttachItemModel>> GetAdminAccountMailAttachItemAsync(long amuid)
        {
            const string cmd = "tp_admin_mail_target_a_reward_by_amuid_select";
            var param = new DynamicParameters();
            param.Add("_amuid", amuid);

            using var conn = OpenGameDB();
            var ret = await conn.QueryAsync<AdminMailAttachItemModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<List<AdminMailModel>> GetAdminCharacterMailAsync()
        {
            const string cmd = "tp_admin_mail_target_c_select";
            var param = new DynamicParameters();

            using var conn = OpenGameDB();
            var ret = await conn.QueryAsync<AdminMailModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<List<AdminMailAttachItemModel>> GetAdminCharacterMailAttachItemAsync(long amuid)
        {
            const string cmd = "tp_admin_mail_target_c_reward_by_amuid_select";
            var param = new DynamicParameters();
            param.Add("_amuid", amuid);

            using var conn = OpenGameDB();
            var ret = await conn.QueryAsync<AdminMailAttachItemModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }
        #endregion        

        public async Task<bool> IsExistsAuid(long auid)
        {
            const string cmd = "tp_admin_mail_auid_select";
            var param = new DynamicParameters();
            param.Add("_auid", auid);

            using var conn = OpenUserDB();
            var ret = await conn.QuerySingleOrDefaultAsync<long>(cmd, param, commandType: CommandType.StoredProcedure);
            return auid == ret;
        }

        public async Task<long> GetAirdropCollectAmount(int serverId, GameType gameType)
        {
            const string cmd = "tp_admin_airdrop_amount";
            var param = new DynamicParameters();
            param.Add("_server_id", serverId);
            param.Add("_game_type", gameType);

            using var conn = OpenGameDB();
            return await conn.QuerySingleOrDefaultAsync<long>(cmd, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<AirdropEventModel>> GetAirdropEventAsync()
        {
            const string cmd = "tp_event_airdrop_select";
            var param = new DynamicParameters();

            using var conn = OpenAdminDB();
            var ret = await conn.QueryAsync<AirdropEventModel>(cmd, param, commandType: CommandType.StoredProcedure);
            return ret.ToList();
        }

        public async Task<bool> AddAirdropEventAsync(AirdropEventModel aes)
        {
            const string cmd = "tp_event_airdrop_insert";
            var param = new DynamicParameters();
            param.Add("_no", aes.No);
            param.Add("_type", aes.EventGameType);
            param.Add("_note", aes.Note);
            param.Add("_use", aes.UseAmount);
            param.Add("_req", aes.RequireAmount);
            param.Add("_begin_ts", aes.BeginTimeStamp);

            using var conn = OpenAdminDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<bool> RemoveAirdropEventAsync(int no)
        {
            const string cmd = "tp_event_airdrop_delete";
            var param = new DynamicParameters();
            param.Add("_no", no);

            using var conn = OpenAdminDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }

        public async Task<bool> SetAirdropEventResultAsync(int no, EventResultType eventRet, string msg)
        {
            const string cmd = "tp_event_airdrop_exec_update";
            var param = new DynamicParameters();
            param.Add("_no", no);
            param.Add("_ret", eventRet);
            param.Add("_msg", msg);

            using var conn = OpenAdminDB();
            var ret = await conn.ExecuteAsync(cmd, param, commandType: CommandType.StoredProcedure);
            return ret == 1;
        }
    }
}
