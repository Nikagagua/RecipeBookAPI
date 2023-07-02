﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using RecipeBook.ServiceLibrary.Entities;

namespace RecipeBook.ServiceLibrary.Repositories
{
    public interface IInstructionRepository
    {
        Task<int> DeleteAsync(
            SqlConnection connection,
            DbTransaction transaction,
            Guid recipeId);
        Task<int> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            IEnumerable<InstructionEntity> entities);
    }

    public class InstructionRepository : IInstructionRepository
    {
        public async Task<int> DeleteAsync(
            SqlConnection connection,
            DbTransaction transaction,
            Guid recipeId)
        {
            var rowsAffected = await connection.ExecuteAsync(@"
				DELETE FROM [dbo].[Instructions]
				WHERE [RecipeId] = @RecipeId",
            new
            {
                RecipeId = recipeId,
            }, transaction: transaction);
            return rowsAffected;
        }

        public async Task<int> InsertAsync(
            SqlConnection connection,
            DbTransaction transaction,
            IEnumerable<InstructionEntity> entities)
        {
            if (entities is null)
            {
                return 0;
            }

            var rowsAffected = 0;
            foreach (var entity in entities)
            {
                rowsAffected += await connection.ExecuteAsync(@"
				INSERT INTO [dbo].[Instructions]
					([RecipeId]
					,[OrdinalPosition]
					,[Instruction])
				VALUES
					(@RecipeId
					,@OrdinalPosition
					,@Instruction)",
                new
                {
                    entity.RecipeId,
                    entity.OrdinalPosition,
                    entity.Instruction,
                }, transaction: transaction);
            }
            return rowsAffected;
        }
    }
}